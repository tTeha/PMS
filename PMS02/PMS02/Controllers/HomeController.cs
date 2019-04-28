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

namespace PMS02.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize(Roles = "Customer,Admin,PM,TL,JE")]
        public ActionResult Index()
        {
            List<object> mylsit = new List<object>();

            MyModel db = new MyModel();


            var result = from y in db.Post
                         where (
                                     from x in db.Responding_Post

                                     select x.Post_ID
                                 ).Contains(y.postID)
                         select y;

            var z = from f in result
                    where !
  (
  from p in db.Project
  select p.postID


  ).Contains(f.userID)
                    select f;

            mylsit.Add(z.ToList());
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
        public ActionResult register([Bind(Exclude = "IsEmailVerified,ActivationCode")]  User user)
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

                //#region // Convert a photo to binary 
                //byte[] data = new byte[user.Photo.ContentLength];
                //user.Photo.InputStream.Read(data, 0, user.Photo.ContentLength);
                user.Photo = "data";
                //#endregion



                #region // save to the database 
                using (MyModel db = new MyModel())
                {

                    db.User.Add(user);
                    db.SaveChanges();

                    message = "Registeraton successfully done !we have sent an activation link to your Email " + user.Email;
                    Status = true;
                }

                #endregion

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

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(User user, string Retunurl)
        {

            using (MyModel db = new MyModel())
            {
                var v = db.User.FirstOrDefault(a => a.Email == user.Email);

                if (v != null)
                {
                    if (string.Compare(crypto.Hash(user.password), v.password) == 0)
                    {

                        Session["First"] = v.First_Name;
                        Session["Last"] = v.Last_Name;
                        Session["Mobile"] = v.Mobile;
                        Session["jop"] = v.Job_Description;
                        Session["photo"] = v.Photo;
                        Session["id"] = v.userID;
                        Session["Role"] = v.Type;
                        Session["Email"] = v.Email;


                        if (Url.IsLocalUrl(Retunurl))
                        {
                            Redirect(Retunurl);

                        }
                        else
                        {


                            if (v.Type == "Admin")
                                return RedirectToAction("Index", "Admin");
                            else if (v.Type == "Customer")
                                return RedirectToAction("Index", "Customer");
                            else if (v.Type == "PM")
                                return RedirectToAction("Index", "PM");
                            else if (v.Type == "TL")
                                return RedirectToAction("Index", "TL");
                            else if (v.Type == "JE")
                                return RedirectToAction("Index", "JE");



                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email address or password is wrong");

                    }

                }
                else
                {
                    ModelState.AddModelError("", "Email address or password is wrong");
                }
            }


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
            using (MyModel db = new MyModel())
            {
                var v = db.User.FirstOrDefault(a => a.Email == EmailID);
                return v != null;
            }
        }


    }
}