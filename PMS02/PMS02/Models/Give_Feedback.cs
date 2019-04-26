namespace PMS02.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Give_Feedback
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? Team_Leader_ID { get; set; }

        public int? Project_Manager_ID { get; set; }

        [StringLength(500)]
        public string Feedback { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
