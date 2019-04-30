namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class b : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "Photo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Photo", c => c.String(nullable: false, maxLength: 200, fixedLength: true));
        }
    }
}
