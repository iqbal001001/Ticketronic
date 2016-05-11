using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ticketronic.Domain;

namespace Ticketronic.WebAPI
{
    public static class TransactionExtension
    {
        public static TransactionDTO ToDTO(this Transaction transaction)
        {
            return new TransactionDTO
            {
                Id = transaction.Id,
                Email = transaction.Email,
                FirstName = transaction.FirstName != null ? transaction.FirstName.Trim() : null,
                LastName = transaction.LastName != null ? transaction.LastName.Trim() : null,
                ContactNo = transaction.ContactNo != null ? transaction.ContactNo.Trim() : null,
                Tickets = transaction.TicketPurchases != null ? transaction.TicketPurchases.Select(t => t.ToDTO()).ToList() : new List<TicketPurchaseDTO>(),

            };
        }

        public static Transaction ToDomain(this TransactionDTO transaction, Transaction originalTransaction = null)
        {
            if (originalTransaction != null && originalTransaction.Id == transaction.Id)
            {
                originalTransaction.Email = transaction.Email;
                originalTransaction.FirstName = transaction.FirstName;
                originalTransaction.LastName = transaction.LastName;
                originalTransaction.ContactNo = transaction.ContactNo;
                originalTransaction.TicketPurchases = transaction.Tickets != null ?
                        transaction.Tickets.Select(s => s.ToDomain(
                        originalTransaction.TicketPurchases != null ? originalTransaction.TicketPurchases.FirstOrDefault(ot => ot.Id == s.Id) : null
                        )).ToList() : new List<TicketPurchase>();


                return originalTransaction;
            }
            return new Transaction
            {
                Id = transaction.Id,
                Email = transaction.Email,
                FirstName = transaction.FirstName,
                LastName = transaction.LastName,
                ContactNo = transaction.ContactNo,
                TicketPurchases = transaction.Tickets != null ? transaction.Tickets.Select(t => t.ToDomain()).ToList() : new List<TicketPurchase>()

            };
        }
    }
}