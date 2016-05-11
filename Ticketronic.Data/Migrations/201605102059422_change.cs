namespace Ticketronic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TicketPurchase", "TicketId", "dbo.Ticket");
            DropForeignKey("dbo.TicketPurchase", "TransactionId", "dbo.Transaction");
            DropIndex("dbo.TicketPurchase", new[] { "TransactionId" });
            RenameColumn(table: "dbo.TicketPurchase", name: "TransactionId", newName: "Transaction_Id");
            AlterColumn("dbo.TicketPurchase", "Transaction_Id", c => c.Int());
            CreateIndex("dbo.TicketPurchase", "Transaction_Id");
            AddForeignKey("dbo.TicketPurchase", "TicketId", "dbo.Ticket", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TicketPurchase", "Transaction_Id", "dbo.Transaction", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketPurchase", "Transaction_Id", "dbo.Transaction");
            DropForeignKey("dbo.TicketPurchase", "TicketId", "dbo.Ticket");
            DropIndex("dbo.TicketPurchase", new[] { "Transaction_Id" });
            AlterColumn("dbo.TicketPurchase", "Transaction_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.TicketPurchase", name: "Transaction_Id", newName: "TransactionId");
            CreateIndex("dbo.TicketPurchase", "TransactionId");
            AddForeignKey("dbo.TicketPurchase", "TransactionId", "dbo.Transaction", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TicketPurchase", "TicketId", "dbo.Ticket", "Id");
        }
    }
}
