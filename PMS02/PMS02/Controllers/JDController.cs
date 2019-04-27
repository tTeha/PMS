using PMS02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS02.Controllers
{
    public class JDController : Controller
    {
        // GET: JD
        private MyModel db = new MyModel();

        //[Authorize(Roles = "JD")]
        public ActionResult Index()
        {
            Session["id"] = 5;
            List<object> TL = new List<object>();
            var id = (int)Session["id"];

            //  var req = from r in db.Sending_Requests where r.Reciever_ID == id select r;
            var result = from y in db.Sending_Request
                         where !(
                                     from x in db.Responding_Request

                                     select x.Request_ID
                                 ).Contains(y.ID)
                               && y.Reciever_ID == id
                         select y;

            TL.Add(result.ToList());
            var project = from c in db.Project
                          where (

                                from y in db.Sending_Request
                                where (
                                    from x in db.Responding_Request
                                    where x.User_ID == id

                                    select x.Request_ID
                                ).Contains(y.ID)
                                select y.Project_ID).Contains(c.projectID)
                          select c;

            TL.Add(project.ToList());

            return View(TL);
        }

        [HttpPost]
        public ActionResult AcceptORreject(int requestid, int userid, bool stat, Responding_Request respond)
        {
            respond.Request_ID = requestid;
            respond.User_ID = userid;
            respond.Respond = stat;
            db.Responding_Request.Add(respond);
            db.SaveChanges();


            return RedirectToAction("Index", "JD");
        }

        
    }
}