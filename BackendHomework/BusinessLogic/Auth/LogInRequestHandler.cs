using System;
using System.Threading.Tasks;
using BackendHomework.Models;
using BackendHomework.Services.Interfaces;

namespace BackendHomework.BusinessLogic.Auth
{
    public class LogInRequestHandler
    {
        private readonly IUserService _userService;
        public LogInRequestHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<User> Handle(LoginCredentials remoteCredentials)
        {
            UserDto dbUser = await _userService.GetUser(remoteCredentials.Username);
            Password pass = new Password(remoteCredentials.Password, dbUser.Id);
            if (!pass.EqualsTo(dbUser.Password))
                throw new Exception("Invalid password!");
            if(dbUser.Status.Equals("deleted"))
                throw new Exception("Your profile is closed!");
            return new User(dbUser.Id, dbUser.Username, 
                dbUser.Password, dbUser.Name, dbUser.Status);
        }
        
    }
}