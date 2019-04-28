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
            List<object> project = new List<object>();

            //  var projects = db.Projects.Include(p => p.Post).Include(p => p.User);
            var id = 1;
            var v = from c in db.Project where c.Post.userID == id select c;
            project.Add(v.ToList());
            var request = from req in db.Sending_Request where req.Reciever_ID == id select req;
            var filter = from f in request
                         where !
                         (
                             from p in db.Project
                             select p.postID


                         ).Contains(f.Project_ID)
                         select f;
            project.Add(filter.ToList());

            var result = from y in db.Post
                         where (
                             from x in db.Responding_Post

                             select x.Post_ID
                         ).Contains(y.postID)
                         select y;
            var userid = 1;
            var filterpost = from filterpos in result
                             where filterpos.userID == userid && !(
                                       from x in db.Project

                                       select x.postID
                                   ).Contains(filterpos.postID)

                             select filterpos;

            var user = from u in db.User where u.Job_Description == "PM" select u;
            project.Add(filterpost.ToList());
            project.Add(user.ToList());
            return View(project);
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
        public ActionResult AcceptRequest(int postid, int ManagerID, int requestid, Project project)
        {
            project.postID = postid;
            project.Project_Manager_ID = ManagerID;
            project.status = "On Progress";
            db.Project.Add(project);
            Sending_Request req = db.Sending_Request.Find(requestid);
            db.Sending_Request.Remove(req);
            db.SaveChanges();



            return RedirectToAction("Index", "Customer");
        }

        //assign project ot project manager
        [HttpPost]
        public ActionResult AssigntoPM(int postid, int managerid, string stat, Project project)
        {
            project.postID = postid;
            project.Project_Manager_ID = managerid;
            project.status = stat;
            db.Project.Add(project);
            db.SaveChanges();

            return RedirectToAction("Index", "Customer");
        }
        //conceling the use request
        [HttpPost]
        public ActionResult DeleteRequest(int requestid, Sending_Request req)
        {

            req = db.Sending_Request.Find(requestid);
            db.Sending_Request.Remove(req);
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