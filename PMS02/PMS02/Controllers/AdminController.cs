using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PMS02.Models;
using System.Net.Mail;
namespace PMS02.Controllers
{
    public class AdminController : Controller
    {
        // GET
        private MyModel db = new MyModel();

        public ActionResult Index()
        {
            List<object> mymodel = new List<object>();
            mymodel.Add(db.User.ToList());
            var result = from y in db.Post
                         where !(
                             from x in db.Responding_Post

                             select x.Post_ID
                         ).Contains(y.userID)
                         select y;
            mymodel.Add(result.ToList());


            return View(mymodel);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "")] User user)
        {


            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {

                #region // Email validation 
                var isexsist = ISemailExisit(user.Email);
                if (isexsist)
                {
                    ModelState.AddModelError("Exist", "Email Already Exsit");
                    return View(user);
                }

                #endregion

                #region // password Hashing
                user.password = crypto.Hash(user.password);
                //user.confirmpassword = crypto.Hash(user.confirmpassword);
                #endregion

                #region // Convert a photo to binary 
                //byte[] data = new byte[user.Photo.ContentLength];
                //user.Photo.InputStream.Read(data, 0, user.Photo.ContentLength);
                #endregion


                Console.WriteLine(user);
                #region // save to the database 
                using (MyModel db = new MyModel())
                {

                    db.User.Add(user);
                    db.SaveChanges();
                    Status = true;
                }
                #endregion

            }



            else
            {
                message = "Invalid Request";
            }

            ViewBag.Status = Status;
            return View(user);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);

            db.SaveChanges();


            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Approveposts(int postid, Responding_Post respond)
        {
            var adminid = Session["id"];
            respond.Admin_ID = (int)adminid;
            respond.Post_ID = postid;
            respond.post_stat = "Approved";
            if (ModelState.IsValid)
            {
                db.Responding_Post.Add(respond);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Admin");

        }

        [HttpPost]
        public ActionResult Deleteposts(int postid)
        {
            Post post = db.Post.Find(postid);
            db.Post.Remove(post);
            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [NonAction]
        public bool ISemailExisit(string EmailID)
        {
            using (MyModel db = new MyModel())
            {
                var v = db.User.Where(a => a.Email == EmailID).FirstOrDefault();
                return v != null;
            }
        }


    }
}