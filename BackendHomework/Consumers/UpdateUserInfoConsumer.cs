using System;
using System.Threading.Tasks;
using BackendHomework.BusinessLogic.Auth;
using BackendHomework.Commands;
using BackendHomework.Services.Interfaces;
using MassTransit;

namespace BackendHomework.Consumers
{
    public class UpdateUserInfoConsumer : IConsumer<UpdateUserInfoCommand>
    {
        private readonly IUserService _userService;

        public UpdateUserInfoConsumer(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<UpdateUserInfoCommand> context)
        {
            if(context.Message.Username != null && !User.isValidEmail(context.Message.Username))
                throw new Exception("Invalid email!");
            if(context.Message.Name != null && context.Message.Name.Length < 3)
                throw new Exception("Name is too short!");
            
            await _userService.UpdateUserInfo(context.Message.Id, context.Message.Username, context.Message.Name);
        }
    }
}