using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS02.Models
{
    public class Responding_Request
    {
        public int ID { get; set; }
        public int User_ID { get; set; }
        public int Request_ID { get; set; }
        public bool Respond { get; set; }

        public virtual User User { get; set; }
        public virtual Sending_Request Sending_Request { get; set; }
    }
}