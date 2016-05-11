namespace Ticketronic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20),
                        Date = c.DateTime(nullable: false),
                        Starttime = c.Time(nullable: false, precision: 7),
                        Duration = c.Single(nullable: false),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Event", t => t.Event_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20),
                        Price = c.Single(nullable: false),
                        Availability = c.Int(nullable: false),
                        MaxPurchase = c.Int(nullable: false),
                        MinPurchase = c.Int(nullable: false),
                        Transaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transaction", t => t.Transaction_Id)
                .Index(t => t.Transaction_Id);
            
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        FirstName = c.String(maxLength: 20),
                        Lastname = c.String(maxLength: 20),
                        ContactNo = c.String(maxLength: 10),
                        Email = c.String(),
                        TimeStamp = c.Binary(fixedLength: true, timestamp: true, storeType: "timestamp"),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ticket", "Transaction_Id", "dbo.Transaction");
            DropForeignKey("dbo.TicketSession", "Session_Id", "dbo.Session");
            DropForeignKey("dbo.TicketSession", "Ticket_Id", "dbo.Ticket");
            DropForeignKey("dbo.Session", "Event_Id", "dbo.Event");
            DropIndex("dbo.TicketSession", new[] { "Session_Id" });
            DropIndex("dbo.TicketSession", new[] { "Ticket_Id" });
            DropIndex("dbo.Ticket", new[] { "Transaction_Id" });
            DropIndex("dbo.Session", new[] { "Event_Id" });
            DropTable("dbo.TicketSession");
            DropTable("dbo.Transaction");
            DropTable("dbo.Ticket");
            DropTable("dbo.Session");
            DropTable("dbo.Event");
        }
    }
}
