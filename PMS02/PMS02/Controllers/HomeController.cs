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
        //[Authorize(Roles = "Customer,Admin,PM,TL,JE")]
        public ActionResult Index()
        {
            List<object> mylsit = new List<object>();
            /*
             var result = from y in db.Post
                          where (
                                 from x in db.Responding_Post
                                  select x.Post_ID
                                  ).Contains(y.postID)
                          select y;

             var z = from f in result
                     where
                     !(
                      from p in db.Project
                      select p.postID
                      ).Contains(f.userID)
                     select f;

             mylsit.Add(z.ToList());
             */
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

        [HttpGet]
        public ActionResult login()
        {
            /*
            if (Session["First"] == null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
            */
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

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("login", "Home");

        }

        [HttpGet]
        public ActionResult PostNewProject()
        {

            return View();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult PostNewProject(Post post)
        {
            var role = Session["Role"];
            var id = Session["id"];
            post.userID = (int)id;
            if (ModelState.IsValid)
            {
                if ((string)role != "Customer")
                {
                    ModelState.AddModelError("Not Allowd", "You are not allowed to post a project");
                    return View();
                }
                else
                    using (MyModel db = new MyModel())
                    {
                        db.Post.Add(post);
                        db.SaveChanges();
                    }


            }
            return RedirectToAction("Index", "Home");


        }
        [NonAction]
        public bool ISemailExisit(string EmailID)
        {
            
            
                var v = db.User.FirstOrDefault(a => a.Email == EmailID);
                return v != null;
            
        }


    }
}