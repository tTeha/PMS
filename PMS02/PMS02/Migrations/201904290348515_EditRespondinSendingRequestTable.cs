namespace PMS02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditRespondinSendingRequestTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sending_Request", "Respond", c => c.Boolean());
            DropColumn("dbo.Sending_Request", "respone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sending_Request", "respone", c => c.String(maxLength: 200, fixedLength: true));
            DropColumn("dbo.Sending_Request", "Respond");
        }
    }
}
