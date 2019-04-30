namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "Type_ID", "dbo.UserTypes");
            DropIndex("dbo.User", new[] { "Type_ID" });
            AlterColumn("dbo.UserTypes", "Role", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.User", "Type_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Type_ID", c => c.Int());
            AlterColumn("dbo.UserTypes", "Role", c => c.String(nullable: false, maxLength: 200, fixedLength: true));
            CreateIndex("dbo.User", "Type_ID");
            AddForeignKey("dbo.User", "Type_ID", "dbo.UserTypes", "ID");
        }
    }
}
