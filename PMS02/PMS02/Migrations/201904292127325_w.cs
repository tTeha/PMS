namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class w : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Role = c.String(nullable: false, maxLength: 200, fixedLength: true),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.User", "IsEmailVerified");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "IsEmailVerified", c => c.String(nullable: false, maxLength: 200, fixedLength: true));
            DropTable("dbo.UserTypes");
        }
    }
}
