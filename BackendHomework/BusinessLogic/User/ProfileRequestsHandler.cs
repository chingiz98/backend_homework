using System;
using System.Threading.Tasks;
using BackendHomework.Models;
using BackendHomework.Services.Interfaces;


namespace BackendHomework.BusinessLogic.User
{
    public class ProfileRequestsHandler
    {
        private readonly IUserService _userService;

        public ProfileRequestsHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task HandleDelete(Guid id)
        {
            return _userService.CloseUserProfile(id);
        }
        
        public async Task<UserDto> HandleGetInfo(Guid id)
        {
            return await _userService.GetUserById(id);
        }
    }
}