namespace PMS02.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sending_Request
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? Sender_ID { get; set; }

        public int? Reciever_ID { get; set; }

        public int? Project_ID { get; set; }

        [StringLength(200)]
        public string respone { get; set; }

        public virtual Project Project { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
