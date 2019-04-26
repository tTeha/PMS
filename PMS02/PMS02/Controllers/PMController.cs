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

            ViewBag.postID = new SelectList(db.Post, "postID", "post_desc", project.postID);
            ViewBag.Project_Manager_ID = new SelectList(db.User, "usrID", "User_Name", project.Project_Manager_ID);
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
            ViewBag.postID = new SelectList(db.Post, "postID", "post_desc", project.postID);
            ViewBag.Project_Manager_ID = new SelectList(db.User, "usrID", "User_Name", project.Project_Manager_ID);
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

        // GET: PM/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PM/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PM/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PM/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PM/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PM/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PM/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
