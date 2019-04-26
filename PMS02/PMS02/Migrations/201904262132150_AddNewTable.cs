namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTable : DbMigration
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
                        Respond = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Sending_Request", t => t.Request_ID)
                .ForeignKey("dbo.User", t => t.User_ID)
                .Index(t => t.User_ID)
                .Index(t => t.Request_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Responding_Request", "User_ID", "dbo.User");
            DropForeignKey("dbo.Responding_Request", "Request_ID", "dbo.Sending_Request");
            DropIndex("dbo.Responding_Request", new[] { "Request_ID" });
            DropIndex("dbo.Responding_Request", new[] { "User_ID" });
            DropTable("dbo.Responding_Request");
        }
    }
}
