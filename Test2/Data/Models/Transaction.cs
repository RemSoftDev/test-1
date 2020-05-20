using System;

namespace Test2.Data.Models
{
    public class Transaction : IEntity<string>
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
