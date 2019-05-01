using PMS02.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PMS02.Controllers
{
    public class PMController : Controller
    {
        // GET: PM
        private MyModel db = new MyModel();

        //[Authorize(Roles = "PM")]
        public ActionResult Index()
        {
            if (Session["id"] != null)
            {
                List<object> listprojects = new List<object>();
                var id = (int)Session["id"];

                MyModel db = new MyModel();
                listprojects.Add(db.Project.ToList());

                var user = from u in db.User where u.Job_Description.Trim() == "Team Leader" || u.Job_Description.Trim() == "Junior Developer" select u;
                listprojects.Add(user.ToList());

                var freePosts = from y in db.Post
                                where
                                !(
                                   from x in db.Asign_Project
                                   where x.Project_Manager_ID == id
                                   select x.post_ID
                                ).Contains(y.postID)
                                select y;
                listprojects.Add(freePosts.ToList());
                return View(listprojects);
            }
            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        public ActionResult Leave(int projectId)
        {
            Project project = db.Project.Find(projectId);
            if (project == null)
            {
                return HttpNotFound();
            }
            db.Project.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index", "PM");
        }

        [HttpGet]
        public ActionResult SetStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.postID = new SelectList(db.Post, "postID", "post_header", project.postID);
            ViewBag.Project_Manager_ID = new SelectList(db.User, "userID", "User_Name", project.Project_Manager_ID);
            return View(project);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetStatus(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.postID = new SelectList(db.Post, "ID", "post_header", project.postID);
            ViewBag.Project_Manager_ID = new SelectList(db.User, "userID", "User_Name", project.Project_Manager_ID);
            return View(project);
        }


        [HttpGet]
        public ActionResult Comment()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Comment(int postid, Comment comment)
        {
            comment.Project_Manager_ID = 2;
            comment.Post_ID = postid;
            if (ModelState.IsValid)
            {
                db.Comment.Add(comment);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "PM");
        }

        [HttpPost]
        public ActionResult SendingRequestToCustomer(int userid, int projectid, Sending_Request request)
        {
            request.Sender_ID = (int)Session["id"];
            request.Reciever_ID = userid;
            request.Project_ID = projectid;
            db.Sending_Request.Add(request);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SendRequest(int senderid, int prjectId, Sending_Request send)
        {

            var v = Request["mail"];
            var mail = Session["Email"];
            if (v != (string)mail)
            {
                var f = db.User.Where(e => e.Email == v).FirstOrDefault(); 
                send.Sender_ID = senderid;
                send.Project_ID = prjectId;
                send.Reciever_ID = f.userID;
                send.Respond = false;
                db.Sending_Request.Add(send);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "PM");

        }

        public ActionResult getMember(int projectId)
        {
            if (Session["id"] != null)
            {
                List<object> member = new List<object>();
                var result = from w in db.User
                             where
                                (from y in db.Sending_Request
                                 where y.Respond == true
                                       && y.Project_ID == projectId
                                 select y.Reciever_ID
                                 ).Contains(w.userID)
                             select w;

                member.Add(result.ToList());
                return View(member);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult removeMember(int? userId, int? projectId)
        {
            if (userId == null && projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (projectId == null && userId == null)
            {
                return RedirectToAction("getMember", "PM");
            }
            var respond_id = from c in db.Sending_Request
                             where
                                c.Project_ID == projectId
                                && c.Reciever_ID == userId
                             select c.ID;
            respond_id.ToList();
            Sending_Request response = db.Sending_Request.Find(respond_id.First());
           
            db.Sending_Request.Remove(response);
            db.SaveChanges();
            return RedirectToAction("Index", "PM");
        }

        [HttpPost]
        public ActionResult ApplyPost(int senderid, int postId, Asign_Project send)
        {

            var v = Request["mail"];
            var mail = Session["Email"];
            if (v != (string)mail)
            {
                var f = db.User.Where(e => e.Email == v).FirstOrDefault();
                if (f == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                send.Project_Manager_ID = senderid;
                send.post_ID = postId;
                send.UserID = f.userID;
                send.Respond = false;
                db.Asign_Project.Add(send);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "PM");

        }

    }
}
