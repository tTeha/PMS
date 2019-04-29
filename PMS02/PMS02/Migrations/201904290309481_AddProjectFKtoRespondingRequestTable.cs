namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectFKtoRespondingRequestTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Responding_Request", "Project_ID", c => c.Int());
            CreateIndex("dbo.Responding_Request", "Project_ID");
            AddForeignKey("dbo.Responding_Request", "Project_ID", "dbo.Project", "projectID");
            DropColumn("dbo.User", "confirmpassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "confirmpassword", c => c.String(nullable: false, maxLength: 200));
            DropForeignKey("dbo.Responding_Request", "Project_ID", "dbo.Project");
            DropIndex("dbo.Responding_Request", new[] { "Project_ID" });
            DropColumn("dbo.Responding_Request", "Project_ID");
        }
    }
}
