using System;

namespace BackendHomework.Commands
{
    public class UpdateUserInfoCommand
    {
        public Guid Id { get; set; } 
        public string Username { get; set; } 
        public string Name { get; set; }
    }
}