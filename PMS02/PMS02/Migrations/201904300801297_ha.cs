namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ha : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Responding_Request",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        User_ID = c.Int(),
                        Request_ID = c.Int(),
                        Project_ID = c.Int(),
                        Respond = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Sending_Request", t => t.rRequest_ID)
                .ForeignKey("dbo.Project", t => t.Project_ID)
                .ForeignKey("dbo.User", t => t.User_ID)
                .Index(t => t.User_ID)
                .Index(t => t.Request_ID)
                .Index(t => t.Project_ID);
            
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Role = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Asign_Project", "Respond", c => c.Boolean());
            AddColumn("dbo.Sending_Request", "Respond", c => c.Boolean());
            AlterColumn("dbo.User", "photo", c => c.String());
            DropColumn("dbo.User", "Type");
            DropColumn("dbo.Sending_Request", "respone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sending_Request", "respone", c => c.String(maxLength: 200, fixedLength: true));
            AddColumn("dbo.User", "Type", c => c.String(nullable: false, maxLength: 200, fixedLength: true));
            DropForeignKey("dbo.Responding_Request", "User_ID", "dbo.User");
            DropForeignKey("dbo.Responding_Request", "Project_ID", "dbo.Project");
            DropForeignKey("dbo.Responding_Request", "Request_ID", "dbo.Sending_Request");
            DropIndex("dbo.Responding_Request", new[] { "Project_ID" });
            DropIndex("dbo.Responding_Request", new[] { "Request_ID" });
            DropIndex("dbo.Responding_Request", new[] { "User_ID" });
            AlterColumn("dbo.User", "photo", c => c.String(nullable: false, maxLength: 128, fixedLength: true));
            DropColumn("dbo.Sending_Request", "Respond");
            DropColumn("dbo.Asign_Project", "Respond");
            DropTable("dbo.UserTypes");
            DropTable("dbo.Responding_Request");
        }
    }
}
