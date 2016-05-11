namespace Ticketronic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stringlength20to50 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Session", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.Ticket", "Name", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ticket", "Name", c => c.String(maxLength: 20));
            AlterColumn("dbo.Session", "Name", c => c.String(maxLength: 20));
        }
    }
}
