using System;
using System.Threading.Tasks;
using BackendHomework.Services.Interfaces;

namespace BackendHomework.BusinessLogic.User
{
    public class DeleteProfileRequestHandler
    {
        private readonly IUserService _userService;

        public DeleteProfileRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task Handle(Guid id)
        {
            return _userService.CloseUserProfile(id);
        }
    }
}