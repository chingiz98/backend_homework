using System;
using System.Threading.Tasks;
using BackendHomework.Models;
using BackendHomework.Services.Interfaces;

namespace BackendHomework.BusinessLogic.Accounts
{
    public class CloseAccountRequestHandler
    {
        private readonly IAccountsService _accountsService;

        public CloseAccountRequestHandler(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        public async Task<AccountDTO> Handle(long id, Guid ownerId)
        {
            AccountDTO accountForClose = await _accountsService.GetAccountById(id);
            if(!accountForClose.Owner_id.Equals(ownerId))
                throw new Exception("The account you are trying to close is not yours!");
            if(Math.Abs(accountForClose.Amount) == 0)
                return await _accountsService.CloseAccount(id, ownerId);
            else 
                throw new Exception("Your account is not empty or you have debt on this account!");
        }
    }
}