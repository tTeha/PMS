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
    public class CustomerController : Controller
    {
        //for database connection 
        private MyModel db = new MyModel();
        public ActionResult Index()
        {
            if (Session["id"] != null)
            {
                List<object> project = new List<object>();
                int id = (int)Session["id"];
                var allProjects = from c in db.Project
                                  where c.Post.userID == id
                                  select c;
                project.Add(allProjects.ToList());

                var asign_ = from asign in db.Asign_Project
                             where
                             asign.UserID == id
                             && asign.Respond == false
                             select asign;
                project.Add(asign_.ToList());

                var not_asign_ = from y in db.Post
                                 where
                                 !(
                                    from x in db.Asign_Project
                                    where x.UserID == id
                                    select x.post_ID
                                 ).Contains(y.postID)
                                 select y;
                project.Add(not_asign_.ToList());

                var user = from u in db.User where u.Job_Description == "Project Manager" select u;
                project.Add(user.ToList());

                return View(project);
            }
            return RedirectToAction("login","Home");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "postID,userID")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Deleteposts(int postid)
        {
            var delete = db.Responding_Post.FirstOrDefault(s => s.Post_ID == postid);
            db.Responding_Post.Remove(delete);
            Post post = db.Post.Find(postid);

            db.Post.Remove(post);

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult AcceptRequest(int postid, int ManagerID, int asignId, Project project)
        {
            project.postID = postid;
            project.Project_Manager_ID = ManagerID;
            project.status = "On Progress";
            db.Project.Add(project);
            Asign_Project asign = db.Asign_Project.Find(asignId);
            if (asign == null)
                return HttpNotFound();
            asign.Respond = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Customer");
        }
        
        //conceling the use request
        [HttpPost]
        public ActionResult DeleteRequest(int asignId, Asign_Project asign)
        {

            asign = db.Asign_Project.Find(asignId);
            db.Asign_Project.Remove(asign);
            db.SaveChanges();

            return RedirectToAction("Index", "Customer");
        }

        //assign project ot project manager
        [HttpPost]
        public ActionResult AssigntoPM(int postid, int managerid, Project project, Asign_Project send)
        {
            int id = (int)Session["id"];
            send.Project_Manager_ID = managerid;
            send.post_ID = postid;
            send.UserID = id;
            send.Respond = true;
            db.Asign_Project.Add(send);

            project.postID = postid;
            project.Project_Manager_ID = managerid;
            project.status = "On Progress";
            db.Project.Add(project);
            db.SaveChanges();

            return RedirectToAction("Index", "Customer");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}