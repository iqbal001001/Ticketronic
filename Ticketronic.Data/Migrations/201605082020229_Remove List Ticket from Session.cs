namespace Ticketronic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveListTicketfromSession : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TicketSession", "Ticket_Id", "dbo.Ticket");
            DropForeignKey("dbo.TicketSession", "Session_Id", "dbo.Session");
            DropIndex("dbo.TicketSession", new[] { "Ticket_Id" });
            DropIndex("dbo.TicketSession", new[] { "Session_Id" });
            AddColumn("dbo.Session", "Ticket_Id", c => c.Int());
            CreateIndex("dbo.Session", "Ticket_Id");
            AddForeignKey("dbo.Session", "Ticket_Id", "dbo.Ticket", "Id");
            DropTable("dbo.TicketSession");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TicketSession",
                c => new
                    {
                        Ticket_Id = c.Int(nullable: false),
                        Session_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ticket_Id, t.Session_Id });
            
            DropForeignKey("dbo.Session", "Ticket_Id", "dbo.Ticket");
            DropIndex("dbo.Session", new[] { "Ticket_Id" });
            DropColumn("dbo.Session", "Ticket_Id");
            CreateIndex("dbo.TicketSession", "Session_Id");
            CreateIndex("dbo.TicketSession", "Ticket_Id");
            AddForeignKey("dbo.TicketSession", "Session_Id", "dbo.Session", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TicketSession", "Ticket_Id", "dbo.Ticket", "Id", cascadeDelete: true);
        }
    }
}
