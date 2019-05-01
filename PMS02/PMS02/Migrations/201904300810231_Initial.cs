namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Asign_Project",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Project_Manager_ID = c.Int(),
                        UserID = c.Int(),
                        post_ID = c.Int(),
                        Respond = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Post", t => t.post_ID)
                .ForeignKey("dbo.User", t => t.Project_Manager_ID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.Project_Manager_ID)
                .Index(t => t.UserID)
                .Index(t => t.post_ID);
            
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        postID = c.Int(nullable: false, identity: true),
                        post_header = c.String(nullable: false, maxLength: 200, fixedLength: true),
                        post_desc = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        created_date = c.DateTime(nullable: false),
                        updated_date = c.DateTime(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.postID)
                .ForeignKey("dbo.User", t => t.userID)
                .Index(t => t.userID);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Project_Manager_ID = c.Int(),
                        Post_ID = c.Int(),
                        Comment_Description = c.String(maxLength: 500, fixedLength: true),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.Project_Manager_ID)
                .ForeignKey("dbo.Post", t => t.Post_ID)
                .Index(t => t.Project_Manager_ID)
                .Index(t => t.Post_ID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        userID = c.Int(nullable: false, identity: true),
                        User_Name = c.String(nullable: false, maxLength: 200, fixedLength: true),
                        password = c.String(nullable: false, maxLength: 200, fixedLength: true),
                        Email = c.String(nullable: false, maxLength: 200, fixedLength: true),
                        Job_Description = c.String(nullable: false, maxLength: 200, fixedLength: true),
                        First_Name = c.String(nullable: false, maxLength: 200, fixedLength: true),
                        Last_Name = c.String(nullable: false, maxLength: 200, fixedLength: true),
                        Mobile = c.String(nullable: false, maxLength: 200, fixedLength: true),
                        photo = c.String(),
                    })
                .PrimaryKey(t => t.userID);
            
            CreateTable(
                "dbo.Give_Evluation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Team_leader_ID = c.Int(),
                        Project_Manager_ID = c.Int(),
                        JD_ID = c.Int(),
                        Evaluations = c.String(maxLength: 500, fixedLength: true),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.JD_ID)
                .ForeignKey("dbo.User", t => t.Project_Manager_ID)
                .ForeignKey("dbo.User", t => t.Team_leader_ID)
                .Index(t => t.Team_leader_ID)
                .Index(t => t.Project_Manager_ID)
                .Index(t => t.JD_ID);
            
            CreateTable(
                "dbo.Give_Feedback",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Team_Leader_ID = c.Int(),
                        Project_Manager_ID = c.Int(),
                        Feedback = c.String(maxLength: 500, fixedLength: true),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.Team_Leader_ID)
                .ForeignKey("dbo.User", t => t.Project_Manager_ID)
                .Index(t => t.Team_Leader_ID)
                .Index(t => t.Project_Manager_ID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        projectID = c.Int(nullable: false, identity: true),
                        postID = c.Int(),
                        Project_Manager_ID = c.Int(),
                        status = c.String(nullable: false, maxLength: 200, fixedLength: true),
                    })
                .PrimaryKey(t => t.projectID)
                .ForeignKey("dbo.Post", t => t.postID)
                .ForeignKey("dbo.User", t => t.Project_Manager_ID)
                .Index(t => t.postID)
                .Index(t => t.Project_Manager_ID);
            
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
                .ForeignKey("dbo.Sending_Request", t => t.Request_ID)
                .ForeignKey("dbo.Project", t => t.Project_ID)
                .ForeignKey("dbo.User", t => t.User_ID)
                .Index(t => t.User_ID)
                .Index(t => t.Request_ID)
                .Index(t => t.Project_ID);
            
            CreateTable(
                "dbo.Sending_Request",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Sender_ID = c.Int(),
                        Reciever_ID = c.Int(),
                        Project_ID = c.Int(),
                        Respond = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Project", t => t.Project_ID)
                .ForeignKey("dbo.User", t => t.Sender_ID)
                .ForeignKey("dbo.User", t => t.Reciever_ID)
                .Index(t => t.Sender_ID)
                .Index(t => t.Reciever_ID)
                .Index(t => t.Project_ID);
            
            CreateTable(
                "dbo.Responding_Post",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Post_ID = c.Int(),
                        Admin_ID = c.Int(),
                        post_stat = c.String(maxLength: 100, fixedLength: true),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.Admin_ID)
                .ForeignKey("dbo.Post", t => t.Post_ID)
                .Index(t => t.Post_ID)
                .Index(t => t.Admin_ID);
            
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Role = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Responding_Post", "Post_ID", "dbo.Post");
            DropForeignKey("dbo.Comment", "Post_ID", "dbo.Post");
            DropForeignKey("dbo.Sending_Request", "Reciever_ID", "dbo.User");
            DropForeignKey("dbo.Sending_Request", "Sender_ID", "dbo.User");
            DropForeignKey("dbo.Responding_Request", "User_ID", "dbo.User");
            DropForeignKey("dbo.Responding_Post", "Admin_ID", "dbo.User");
            DropForeignKey("dbo.Project", "Project_Manager_ID", "dbo.User");
            DropForeignKey("dbo.Sending_Request", "Project_ID", "dbo.Project");
            DropForeignKey("dbo.Responding_Request", "Project_ID", "dbo.Project");
            DropForeignKey("dbo.Responding_Request", "Request_ID", "dbo.Sending_Request");
            DropForeignKey("dbo.Project", "postID", "dbo.Post");
            DropForeignKey("dbo.Post", "userID", "dbo.User");
            DropForeignKey("dbo.Give_Feedback", "Project_Manager_ID", "dbo.User");
            DropForeignKey("dbo.Give_Feedback", "Team_Leader_ID", "dbo.User");
            DropForeignKey("dbo.Give_Evluation", "Team_leader_ID", "dbo.User");
            DropForeignKey("dbo.Give_Evluation", "Project_Manager_ID", "dbo.User");
            DropForeignKey("dbo.Give_Evluation", "JD_ID", "dbo.User");
            DropForeignKey("dbo.Comment", "Project_Manager_ID", "dbo.User");
            DropForeignKey("dbo.Asign_Project", "UserID", "dbo.User");
            DropForeignKey("dbo.Asign_Project", "Project_Manager_ID", "dbo.User");
            DropForeignKey("dbo.Asign_Project", "post_ID", "dbo.Post");
            DropIndex("dbo.Responding_Post", new[] { "Admin_ID" });
            DropIndex("dbo.Responding_Post", new[] { "Post_ID" });
            DropIndex("dbo.Sending_Request", new[] { "Project_ID" });
            DropIndex("dbo.Sending_Request", new[] { "Reciever_ID" });
            DropIndex("dbo.Sending_Request", new[] { "Sender_ID" });
            DropIndex("dbo.Responding_Request", new[] { "Project_ID" });
            DropIndex("dbo.Responding_Request", new[] { "Request_ID" });
            DropIndex("dbo.Responding_Request", new[] { "User_ID" });
            DropIndex("dbo.Project", new[] { "Project_Manager_ID" });
            DropIndex("dbo.Project", new[] { "postID" });
            DropIndex("dbo.Give_Feedback", new[] { "Project_Manager_ID" });
            DropIndex("dbo.Give_Feedback", new[] { "Team_Leader_ID" });
            DropIndex("dbo.Give_Evluation", new[] { "JD_ID" });
            DropIndex("dbo.Give_Evluation", new[] { "Project_Manager_ID" });
            DropIndex("dbo.Give_Evluation", new[] { "Team_leader_ID" });
            DropIndex("dbo.Comment", new[] { "Post_ID" });
            DropIndex("dbo.Comment", new[] { "Project_Manager_ID" });
            DropIndex("dbo.Post", new[] { "userID" });
            DropIndex("dbo.Asign_Project", new[] { "post_ID" });
            DropIndex("dbo.Asign_Project", new[] { "UserID" });
            DropIndex("dbo.Asign_Project", new[] { "Project_Manager_ID" });
            DropTable("dbo.UserTypes");
            DropTable("dbo.Responding_Post");
            DropTable("dbo.Sending_Request");
            DropTable("dbo.Responding_Request");
            DropTable("dbo.Project");
            DropTable("dbo.Give_Feedback");
            DropTable("dbo.Give_Evluation");
            DropTable("dbo.User");
            DropTable("dbo.Comment");
            DropTable("dbo.Post");
            DropTable("dbo.Asign_Project");
        }
    }
}
