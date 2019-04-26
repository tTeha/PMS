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
            List<object> listprojects = new List<object>();
            MyModel db = new MyModel();
            listprojects.Add(db.Project.ToList());
            var user = from u in db.User where u.Job_Description == "TL" || u.Job_Description == "JD" select u;
            listprojects.Add(user.ToList());
            return View(listprojects);
        }
        
        [HttpPost]
        public ActionResult Leave(int postid)
        {
            Project project = db.Project.Find(postid);
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
                send.respone = "no";
                db.Sending_Request.Add(send);
                db.SaveChanges();
                
            }


            return RedirectToAction("Index", "PM");

        }
        
    }
}
