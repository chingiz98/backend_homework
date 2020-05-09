using System;

namespace BackendHomework.Models
{
    public class TransactionDTO
    {
        public long to_id { get; set; }
        public long from_id { get; set; }
        public decimal amount { get; set; }
        public string comment { get; set; }
        public string type { get; set; }
        public DateTime timestamp { get; set; }
    }
}