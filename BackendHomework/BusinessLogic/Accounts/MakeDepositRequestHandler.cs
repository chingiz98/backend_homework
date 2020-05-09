using System;
using System.Threading.Tasks;
using BackendHomework.Commands;
using MassTransit;

namespace BackendHomework.BusinessLogic.Accounts
{
    public class MakeDepositRequestHandler
    {
        //private readonly IAccountsService _accountsService;
        private readonly IBus _bus;
         public MakeDepositRequestHandler(IBus bus)
        {
            _bus = bus;
        }
        /*
        public MakeDepositRequestHandler(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }
        */

        public Task Handle(long id, Guid ownerId, decimal amount)
        {
            return _bus.Send(new MakeDepositCommand
            {
                Id = id,
                Amount = amount,
                Comment = ""
            });
            /*
            if(amount < 0)
                throw new Exception("Deposit amount can't be less than zero");
            return _accountsService.MakeDeposit(id, amount);
            */
        }
    }
}