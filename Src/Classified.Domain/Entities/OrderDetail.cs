using System;
using System.ComponentModel.DataAnnotations;

namespace Classified.Domain.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public string TransactionId { get; set; }

        public int? UserId { get; set; }

        public string PayerEmailId { get; set; }

        public string ReceiverEmailId { get; set; }

        public DateTime PaidDate { get; set; }

        public decimal Amount { get; set; }

        public int SelectedId { get; set; }

        public string PaymentFor { get; set; }
    }
}