namespace PMS02.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Asign_Project
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? Project_Manager_ID { get; set; }

        public int? UserID { get; set; }

        public int? post_ID { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
