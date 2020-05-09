using System;
using System.Text.RegularExpressions;

namespace BackendHomework.BusinessLogic.Auth
{
    public class User
    {

        public User(Guid id, string username, string passwordHash, string name, string status)
        {
            Id = id;
            Username = username;
            _password = new Password(passwordHash);
            Name = name;
            Status = status;
       
        }
        public User(string username, string password, string name)
        {
            Id = Guid.NewGuid();
            Username = username;
            _password = new Password(password, Id);
            Name = name;
            Status = "moderation";
    
        }
        
        private Password _password;
        public string Username { get; set; }
        
        public String Status { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    
        public string Password
        {
            get { return _password.PasswordHash; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _password = new Password(Password);
            }
        }
        
        public static bool isValidEmail(string email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }
    }
    
    
}