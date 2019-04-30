namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class d : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Asign_Project", "Respond", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Asign_Project", "Respond");
        }
    }
}
