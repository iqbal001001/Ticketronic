namespace Ticketronic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyEventIdonSessions : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Session", name: "Event_Id", newName: "EventId");
            RenameIndex(table: "dbo.Session", name: "IX_Event_Id", newName: "IX_EventId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Session", name: "IX_EventId", newName: "IX_Event_Id");
            RenameColumn(table: "dbo.Session", name: "EventId", newName: "Event_Id");
        }
    }
}
