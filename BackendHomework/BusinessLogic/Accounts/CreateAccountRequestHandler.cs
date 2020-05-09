using System;
using System.Threading.Tasks;
using BackendHomework.Models;
using BackendHomework.Services.Interfaces;

namespace BackendHomework.BusinessLogic.Accounts
{
    public class CreateAccountRequestHandler
    {
        private readonly IAccountsService _accountsService;

        public CreateAccountRequestHandler(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        public Task<AccountDTO> Handle(string accountName, Guid ownerId)
        {
            return _accountsService.CreateAccount(accountName, ownerId);
        }
    }
}