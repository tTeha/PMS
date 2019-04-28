namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmigartions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "confirmpassword", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "confirmpassword");
        }
    }
}
