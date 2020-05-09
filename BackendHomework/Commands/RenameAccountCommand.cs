using System;

namespace BackendHomework.Commands
{
    public class RenameAccountCommand
    {
        public long AccountId { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
    }
}