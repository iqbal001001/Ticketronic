namespace Ticketronic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEntityTicketPurchase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ticket", "Transaction_Id", "dbo.Transaction");
            DropIndex("dbo.Ticket", new[] { "Transaction_Id" });
            CreateTable(
                "dbo.TicketPurchase",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Ticket_Id = c.Int(),
                        Transaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ticket", t => t.Ticket_Id)
                .ForeignKey("dbo.Transaction", t => t.Transaction_Id)
                .Index(t => t.Ticket_Id)
                .Index(t => t.Transaction_Id);
            
            DropColumn("dbo.Ticket", "Transaction_Id");
            DropColumn("dbo.Transaction", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transaction", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Ticket", "Transaction_Id", c => c.Int());
            DropForeignKey("dbo.TicketPurchase", "Transaction_Id", "dbo.Transaction");
            DropForeignKey("dbo.TicketPurchase", "Ticket_Id", "dbo.Ticket");
            DropIndex("dbo.TicketPurchase", new[] { "Transaction_Id" });
            DropIndex("dbo.TicketPurchase", new[] { "Ticket_Id" });
            DropTable("dbo.TicketPurchase");
            CreateIndex("dbo.Ticket", "Transaction_Id");
            AddForeignKey("dbo.Ticket", "Transaction_Id", "dbo.Transaction", "Id");
        }
    }
}
