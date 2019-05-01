using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PMS02.Models;
using System.IO;

namespace PMS02.Controllers
{
    public class HomeController : Controller
    {
        MyModel db = new MyModel();
        public ActionResult Index()
        {
            List<object> mylsit = new List<object>();
            
             var result = from y in db.Post
                          where (
                                 from x in db.Responding_Post
                                  select x.Post_ID
                                  ).Contains(y.postID)
                          select y;

            mylsit.Add(result.ToList());
             
            mylsit.Add(db.Sending_Request.ToList());
            return View(mylsit);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult register()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult register(User user)
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
                /*
                string FileName = Path.GetFileNameWithoutExtension(user.photoFile.FileName);
                string FileExtension = Path.GetExtension(user.photoFile.FileName);
                Random rnd = new Random();
                int r = rnd.Next();
                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + r.ToString() + FileName.Trim() + FileExtension;
                string path = Path.Combine(Server.MapPath("~/Content/Images/"), FileName);
                user.photo = "Content/Images" + FileName;
                */
                user.photo = "";
                db.User.Add(user);
                db.SaveChanges();
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

        [HttpGet]
        public ActionResult login()
        {
         
            if (Session["First"] == null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
            
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(User user)
        {
            string message = "";
            var v = db.User.FirstOrDefault(a => a.Email.Trim() == user.Email.Trim());
                if (v != null)
                {
                    if (string.Compare(crypto.Hash(user.password.Trim()), v.password.Trim()) == 0)
                    {
                        Session["First"] = v.First_Name.Trim();
                        Session["Last"] = v.Last_Name.Trim();
                        Session["Mobile"] = v.Mobile.Trim();
                        Session["Job"] = v.Job_Description.Trim();
                        Session["id"] = v.userID;
                        Session["Role"] = v.Job_Description.Trim();
                        Session["Email"] = v.Email.Trim();
                   
                        
                        if (v.Job_Description.Trim() == "Admin")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        if (v.Job_Description.Trim() == "Customer")
                        {
                            return RedirectToAction("Index", "Customer");
                        }
                        if (v.Job_Description.Trim() == "Project Manager")
                        {
                            return RedirectToAction("Index", "PM");
                        }
                        if (v.Job_Description.Trim() == "Team Leader")
                        {
                            return RedirectToAction("Index", "TL");
                        }
                        if (v.Job_Description.Trim() == "Junior Developer")
                        {
                            return RedirectToAction("Index", "JD");
                        }
                    }
                    else
                    {
                        message = "Email address or password is wrong";
                    }
                }
                else
                {
                    message = "Email address or password is wrong";
                }

            ViewBag.Message = message;
            return View();
        }

        
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("login", "Home");

        }

        [HttpGet]
        public ActionResult PostNewProject()
        {
            if (Session["id"] != null)
            {

                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult PostNewProject(Post post)
        {
            if (Session["id"] != null)
            {
                
                var role = Session["Role"];
                var id = Session["id"];
                if ((string)Session["Role"] == "Customer")
                {
                    if (ModelState.IsValid)
                    {

                        post.userID = (int)id;

                        db.Post.Add(post);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index", "Customer");
                }
                return RedirectToAction("Index", "Home");

            }

            return RedirectToAction("login", "Home");

        }
        [NonAction]
        public bool ISemailExisit(string EmailID)
        {
            
            
                var v = db.User.FirstOrDefault(a => a.Email == EmailID);
                return v != null;
            
        }


    }
}