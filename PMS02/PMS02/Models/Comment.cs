namespace PMS02.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? Project_Manager_ID { get; set; }

        public int? Post_ID { get; set; }

        [StringLength(500)]
        public string Comment_Description { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }
    }
}
