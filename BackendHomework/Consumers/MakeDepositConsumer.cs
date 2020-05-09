using System;
using System.Threading.Tasks;
using BackendHomework.Commands;
using BackendHomework.Services.Interfaces;
using MassTransit;

namespace BackendHomework.Consumers
{
    public class MakeDepositConsumer : IConsumer<MakeDepositCommand>
    {
        private readonly IAccountsService _accountsService;

        public MakeDepositConsumer(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        public async Task Consume(ConsumeContext<MakeDepositCommand> context)
        {
            if(context.Message.Amount <= 0)
                throw new Exception("Deposit amount can't be zero or less");
            await _accountsService.MakeDeposit(context.Message.Id, context.Message.Amount);
        }
    }
}