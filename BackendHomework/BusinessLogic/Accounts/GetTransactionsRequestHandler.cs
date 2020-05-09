using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendHomework.Models;
using BackendHomework.Services.Interfaces;

namespace BackendHomework.BusinessLogic.Accounts
{
    public class GetTransactionsRequestHandler
    {
        private readonly IAccountsService _accountsService;

        public GetTransactionsRequestHandler(IAccountsService accountsService) 
        {
            _accountsService = accountsService;
        }

        public async Task<List<TransactionDTO>> Handle(long accountId, Guid ownerId)
        {
            AccountDTO account = await _accountsService.GetAccountById(accountId);
            if(!account.Owner_id.Equals(ownerId))
                throw new Exception("The account you requesting transactions history is not yours!");
            List<TransactionDTO> res = await _accountsService.GetTransactionsByAccountId(accountId);

            return res;
        }
    }
}