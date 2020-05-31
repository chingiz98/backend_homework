using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendHomework.Models;
using BackendHomework.Services.Interfaces;

namespace BackendHomework.BusinessLogic.Accounts
{
    public class GetAccountsRequestHandler
    {
        private readonly IAccountsService _accountsService;

        public GetAccountsRequestHandler(IAccountsService accountsService) 
        {
            _accountsService = accountsService;
        }

        public Task<List<AccountDTO>> HandleGetAccounts(Guid ownerId)
        {
            return _accountsService.GetAccountsByOwnerId(ownerId);
        }
        
        public async Task<AccountDTO> HandleGetAccountById(Guid ownerId, long id)
        {
            AccountDTO acc = await _accountsService.GetAccountById(id);

            if (acc.Owner_id != ownerId)
                throw new Exception("Not your account");
            return acc;
        }
    }
}