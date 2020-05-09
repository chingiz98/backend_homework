using System;
using System.Threading.Tasks;
using BackendHomework.Commands;
using MassTransit;

namespace BackendHomework.BusinessLogic.Accounts
{
    public class RenameAccountRequestHandler
    {
        /*
        private readonly IAccountsService _accountsService;
        public RenameAccountRequestHandler(IAccountsService accountsService) 
        {
            _accountsService = accountsService;
        }
        */
        private readonly IBus _bus;
        public RenameAccountRequestHandler(IBus bus)
        {
            _bus = bus;
        }
        
        //public Task<AccountDTO> Handle(long accountId, Guid ownerId, string name)
        public Task Handle(long accountId, Guid ownerId, string name)
        {
            return _bus.Send(new RenameAccountCommand
            {
                AccountId = accountId,
                OwnerId = ownerId,
                Name = name
            });
            /*
            if (name != null) 
                return await _accountsService.RenameAccount(accountId, ownerId, name);
            else 
                throw new Exception("Name can't be empty");
                */
        }
    }
}