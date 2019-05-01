namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aew : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Post", "created_date");
            DropColumn("dbo.Post", "updated_date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Post", "updated_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Post", "created_date", c => c.DateTime(nullable: false));
        }
    }
}
