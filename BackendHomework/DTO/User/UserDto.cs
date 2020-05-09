using System;

namespace BackendHomework.Models
{
    public class UserDto : SignupCredentials
    {
        public Guid Id { get; set; }
        public string Status { get; set; }

    }
}