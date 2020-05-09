using System;

namespace BackendHomework.Models
{
    public class AccountDTO
    {
        public long Id { get; set; }
        public Guid Owner_id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public Boolean closed { get; set; }
    }
}