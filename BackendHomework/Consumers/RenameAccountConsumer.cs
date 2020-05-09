using System;
using System.Threading.Tasks;
using BackendHomework.Commands;
using BackendHomework.Services.Interfaces;
using MassTransit;

namespace BackendHomework.Consumers
{
    public class RenameAccountConsumer : IConsumer<RenameAccountCommand>
    {
        private readonly IAccountsService _accountsService;

        public RenameAccountConsumer(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        public async Task Consume(ConsumeContext<RenameAccountCommand> context)
        {
            if(context.Message.Name == null || context.Message.Name.Equals(""))
                throw new Exception("Name can't be empty");
            await _accountsService.RenameAccount(context.Message.AccountId, context.Message.OwnerId, context.Message.Name);
        }
    }
}