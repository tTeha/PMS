using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PMS02.Models
{
    public class Responding_Request
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int? User_ID { get; set; }
        public int? Request_ID { get; set; }
        public int? Project_ID { get; set; }
        public bool? Respond { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
        public virtual Sending_Request Sending_Request { get; set; }
    }
}