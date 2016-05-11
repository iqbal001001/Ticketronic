namespace Ticketronic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketPurchasefieldaddedTransactionIdTicketId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TicketPurchase", "Ticket_Id", "dbo.Ticket");
            DropForeignKey("dbo.TicketPurchase", "Transaction_Id", "dbo.Transaction");
            DropIndex("dbo.TicketPurchase", new[] { "Ticket_Id" });
            DropIndex("dbo.TicketPurchase", new[] { "Transaction_Id" });
            RenameColumn(table: "dbo.TicketPurchase", name: "Ticket_Id", newName: "TicketId");
            RenameColumn(table: "dbo.TicketPurchase", name: "Transaction_Id", newName: "TransactionId");
            AlterColumn("dbo.TicketPurchase", "TicketId", c => c.Int(nullable: false));
            AlterColumn("dbo.TicketPurchase", "TransactionId", c => c.Int(nullable: false));
            CreateIndex("dbo.TicketPurchase", "TransactionId");
            CreateIndex("dbo.TicketPurchase", "TicketId");
            AddForeignKey("dbo.TicketPurchase", "TicketId", "dbo.Ticket", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TicketPurchase", "TransactionId", "dbo.Transaction", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketPurchase", "TransactionId", "dbo.Transaction");
            DropForeignKey("dbo.TicketPurchase", "TicketId", "dbo.Ticket");
            DropIndex("dbo.TicketPurchase", new[] { "TicketId" });
            DropIndex("dbo.TicketPurchase", new[] { "TransactionId" });
            AlterColumn("dbo.TicketPurchase", "TransactionId", c => c.Int());
            AlterColumn("dbo.TicketPurchase", "TicketId", c => c.Int());
            RenameColumn(table: "dbo.TicketPurchase", name: "TransactionId", newName: "Transaction_Id");
            RenameColumn(table: "dbo.TicketPurchase", name: "TicketId", newName: "Ticket_Id");
            CreateIndex("dbo.TicketPurchase", "Transaction_Id");
            CreateIndex("dbo.TicketPurchase", "Ticket_Id");
            AddForeignKey("dbo.TicketPurchase", "Transaction_Id", "dbo.Transaction", "Id");
            AddForeignKey("dbo.TicketPurchase", "Ticket_Id", "dbo.Ticket", "Id");
        }
    }
}
