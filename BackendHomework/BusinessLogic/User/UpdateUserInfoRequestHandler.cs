using System;
using System.Threading.Tasks;
using BackendHomework.Commands;
using MassTransit;

namespace BackendHomework.BusinessLogic.User
{
    public class UpdateUserInfoRequestHandler
    {
        /*
        private readonly IUserService _userService;
        public UpdateUserInfoRequestHandler(IUserService userService)
        {
            _userService = userService;
        }
        */
        private readonly IBus _bus;
        public UpdateUserInfoRequestHandler(IBus bus)
        {
            _bus = bus;
        }

        public Task Handle(Guid id, string username, string name)
        {
            return _bus.Send(new UpdateUserInfoCommand
            {
                Id = id,
                Username = username,
                Name = name
            });
            /*
            if(username != null && !Auth.User.isValidEmail(username))
                throw new Exception("Invalid email!");
            if(name.Length < 3)
                throw new Exception("Name is too short!");
            await _userService.UpdateUserInfo(id, username, name);
            */
        }
    }
}