namespace PMS02.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Responding_Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? Post_ID { get; set; }

        public int? Admin_ID { get; set; }

        [StringLength(100)]
        public string post_stat { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }
    }
}
