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
using System.IO;

namespace PMS02.Controllers
{
    public class AdminController : Controller
    {
        // GET
        private MyModel db = new MyModel();

        public ActionResult Index()
        {
            if (Session["id"] != null)
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
            return RedirectToAction("Index", "Home");
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
        public ActionResult Create(User user)
        {
            if (Session["id"] != null)
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
                    #endregion

                    string FileName = Path.GetFileNameWithoutExtension(user.photoFile.FileName);
                    string FileExtension = Path.GetExtension(user.photoFile.FileName);
                    Random rnd = new Random();
                    int r = rnd.Next();
                    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + r.ToString() + FileName.Trim() + FileExtension;
                    string path = Path.Combine(Server.MapPath("~/Content/Images/"), FileName);
                    user.photo = "Content/Images" + FileName;

                    db.User.Add(user);
                    db.SaveChanges();
                    user.photoFile.SaveAs(path);
                    message = "Registeraton successfully done !we have sent an activation link to your Email " + user.Email;
                    Status = true;
                }
                else
                {
                    message = "Invalid Request";
                }
                ViewBag.Message = message;
                ViewBag.Status = Status;
                return View(user);
            }
            return RedirectToAction("Index", "Home");
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