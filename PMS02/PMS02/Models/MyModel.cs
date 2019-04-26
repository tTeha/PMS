namespace PMS02.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyModel : DbContext
    {
        public MyModel()
            : base("name=MyModel")
        {
        }

        public virtual DbSet<Asign_Project> Asign_Project { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Give_Evluation> Give_Evluation { get; set; }
        public virtual DbSet<Give_Feedback> Give_Feedback { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Responding_Post> Responding_Post { get; set; }
        public virtual DbSet<Sending_Request> Sending_Request { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Responding_Request> Responding_Request { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .Property(e => e.Comment_Description)
                .IsFixedLength();

            modelBuilder.Entity<Give_Evluation>()
                .Property(e => e.Evaluations)
                .IsFixedLength();

            modelBuilder.Entity<Give_Feedback>()
                .Property(e => e.Feedback)
                .IsFixedLength();

            modelBuilder.Entity<Post>()
                .Property(e => e.post_header)
                .IsFixedLength();

            modelBuilder.Entity<Post>()
                .Property(e => e.post_desc)
                .IsFixedLength();

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Asign_Project)
                .WithOptional(e => e.Post)
                .HasForeignKey(e => e.post_ID);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Comment)
                .WithOptional(e => e.Post)
                .HasForeignKey(e => e.Post_ID);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Responding_Post)
                .WithOptional(e => e.Post)
                .HasForeignKey(e => e.Post_ID);

            modelBuilder.Entity<Sending_Request>()
                .HasMany(e => e.Responding_Request)
                .WithOptional(e => e.Sending_Request)
                .HasForeignKey(e => e.Request_ID);

            modelBuilder.Entity<Project>()
                .Property(e => e.status)
                .IsFixedLength();

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Sending_Request)
                .WithOptional(e => e.Project)
                .HasForeignKey(e => e.Project_ID);

            modelBuilder.Entity<Responding_Post>()
                .Property(e => e.post_stat)
                .IsFixedLength();

            modelBuilder.Entity<Sending_Request>()
                .Property(e => e.respone)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.User_Name)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Photo)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Job_Description)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.First_Name)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Last_Name)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Mobile)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Type)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.IsEmailVerified)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Asign_Project)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Project_Manager_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Asign_Project1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Comment)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Project_Manager_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Give_Evluation)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.JD_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Give_Evluation1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.Project_Manager_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Give_Evluation2)
                .WithOptional(e => e.User2)
                .HasForeignKey(e => e.Team_leader_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Give_Feedback)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Team_Leader_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Give_Feedback1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.Project_Manager_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Post)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Project)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Project_Manager_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Responding_Post)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Admin_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Sending_Request)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Sender_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Sending_Request1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.Reciever_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Responding_Request)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.User_ID);

        }
    }
}
