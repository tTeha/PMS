using PMS02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS02.Controllers
{
    public class TLController : Controller
    {
        
        private MyModel db = new MyModel();

        //[Authorize(Roles = "TL")]
        public ActionResult Index()
        {
            List<object> TL = new List<object>();
            var id = (int)Session["id"];

            var result = from y in db.Sending_Request
                         where y.Respond == false
                               && y.Reciever_ID == id
                         select y;
            TL.Add(result.ToList());

            var project = from c in db.Project
                          where (
                                from y in db.Sending_Request
                                where y.Respond == true
                                && y.Reciever_ID == id
                                select y.Project_ID
                                ).Contains(c.projectID)
                          select c;
            TL.Add(project.ToList());

            var members = from w in db.User
                         where
                            !(from y in db.Sending_Request
                              where y.Respond == true
                              select y.Reciever_ID
                             ).Contains(w.userID)
                             && w.Job_Description == "JD"
                         select w;
            TL.Add(members.ToList());

            return View(TL);
        }

        [HttpPost]
        public ActionResult AcceptORreject(int requestid, bool stat, Sending_Request respond)
        {
           respond = db.Sending_Request.Find(requestid);
           if (respond == null)
            {
                return HttpNotFound();
            }
            respond.Respond = stat;
            db.SaveChanges();

            return RedirectToAction("Index", "TL");
        }

        [HttpPost]
        public ActionResult Leave(int projectId, int userId)
        {
            var respond_id = from c in db.Sending_Request
                             where
                                c.Project_ID == projectId
                                && c.Reciever_ID == userId
                             select c.ID;
            respond_id.ToList();
            foreach (var item in respond_id)
            {
                Sending_Request response = db.Sending_Request.Find(respond_id.First());
                if (response == null)
                {
                    return HttpNotFound();
                }
                db.Sending_Request.Remove(response);
                
            }
            db.SaveChanges();

            return RedirectToAction("Index", "TL");
        }

        [HttpPost]
        public ActionResult SendRequest(int senderid, int prjectId, Sending_Request send)
        {

            var v = Request["mail"];
            var mail = Session["Email"];
            if (v != (string)mail)
            {
                var f = db.User.Where(e => e.Email == v).FirstOrDefault();
                send.Sender_ID = senderid;
                send.Project_ID = prjectId;
                send.Reciever_ID = f.userID;
                send.Respond = false;
                db.Sending_Request.Add(send);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "TL");
        }
        
        
    }
}