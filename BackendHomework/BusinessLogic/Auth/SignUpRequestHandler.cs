using System;
using System.Threading.Tasks;
using BackendHomework.Models;
using BackendHomework.Services.Interfaces;


namespace BackendHomework.BusinessLogic.Auth
{
    public class SignUpRequestHandler
    {
        
        private readonly IUserService _userService;

        public SignUpRequestHandler(IUserService userService)
        {
            _userService = userService;
        }
        
        
        public async Task<User> Handle(SignupCredentials signupCredentials)
        {
            if(signupCredentials.Username == null || !User.isValidEmail(signupCredentials.Username))
                throw new Exception("Invalid email!");
            if(signupCredentials.Name == null || signupCredentials.Name.Equals(""))
                throw new Exception("Name can't be empty!");
            if(signupCredentials.Password == null || signupCredentials.Password.Equals(""))
                throw new Exception("Password can't be empty!");
            if(signupCredentials.Name.Length < 3)
                throw new Exception("Name is too short!");
            User newUser = new User(signupCredentials.Username, signupCredentials.Password, signupCredentials.Name);
            
            await _userService.AddNewUser(new UserDto()
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Password = newUser.Password,
                Status = newUser.Status,
                Username = newUser.Username
            });
            return newUser;
        }
    }
}