namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class c : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "photo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "photo");
        }
    }
}
