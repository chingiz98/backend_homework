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

        public Task<List<AccountDTO>> Handle(Guid ownerId)
        {
            return _accountsService.GetAccountsByOwnerId(ownerId);
        }
    }
}