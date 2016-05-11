namespace Ticketronic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventCascadeSession : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Session", "Event_Id", "dbo.Event");
            DropIndex("dbo.Session", new[] { "Event_Id" });
            AlterColumn("dbo.Session", "Event_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Session", "Event_Id");
            AddForeignKey("dbo.Session", "Event_Id", "dbo.Event", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Session", "Event_Id", "dbo.Event");
            DropIndex("dbo.Session", new[] { "Event_Id" });
            AlterColumn("dbo.Session", "Event_Id", c => c.Int());
            CreateIndex("dbo.Session", "Event_Id");
            AddForeignKey("dbo.Session", "Event_Id", "dbo.Event", "Id");
        }
    }
}
