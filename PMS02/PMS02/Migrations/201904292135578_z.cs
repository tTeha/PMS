namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class z : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Type_ID", c => c.Int());
            CreateIndex("dbo.User", "Type_ID");
            AddForeignKey("dbo.User", "Type_ID", "dbo.UserTypes", "ID");
            DropColumn("dbo.User", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Type", c => c.String(nullable: false, maxLength: 200, fixedLength: true));
            DropForeignKey("dbo.User", "Type_ID", "dbo.UserTypes");
            DropIndex("dbo.User", new[] { "Type_ID" });
            DropColumn("dbo.User", "Type_ID");
        }
    }
}
