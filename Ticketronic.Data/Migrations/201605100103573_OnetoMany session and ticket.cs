namespace Ticketronic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnetoManysessionandticket : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Session", "Ticket_Id", "dbo.Ticket");
            DropIndex("dbo.Session", new[] { "Ticket_Id" });
            CreateTable(
                "dbo.TicketSession",
                c => new
                    {
                        Ticket_Id = c.Int(nullable: false),
                        Session_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ticket_Id, t.Session_Id })
                .ForeignKey("dbo.Ticket", t => t.Ticket_Id, cascadeDelete: true)
                .ForeignKey("dbo.Session", t => t.Session_Id, cascadeDelete: true)
                .Index(t => t.Ticket_Id)
                .Index(t => t.Session_Id);
            
            DropColumn("dbo.Session", "Ticket_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Session", "Ticket_Id", c => c.Int());
            DropForeignKey("dbo.TicketSession", "Session_Id", "dbo.Session");
            DropForeignKey("dbo.TicketSession", "Ticket_Id", "dbo.Ticket");
            DropIndex("dbo.TicketSession", new[] { "Session_Id" });
            DropIndex("dbo.TicketSession", new[] { "Ticket_Id" });
            DropTable("dbo.TicketSession");
            CreateIndex("dbo.Session", "Ticket_Id");
            AddForeignKey("dbo.Session", "Ticket_Id", "dbo.Ticket", "Id");
        }
    }
}
