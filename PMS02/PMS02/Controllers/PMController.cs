using PMS02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
