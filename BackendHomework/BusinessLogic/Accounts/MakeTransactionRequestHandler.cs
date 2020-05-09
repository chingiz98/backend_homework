using System;
using System.Threading.Tasks;
using BackendHomework.Models;
using BackendHomework.Services.Interfaces;

namespace BackendHomework.BusinessLogic.Accounts
{
    public class MakeTransactionRequestHandler
    {
        private readonly IAccountsService _accountsService;

        public MakeTransactionRequestHandler(IAccountsService accountsService) 
        {
            _accountsService = accountsService;
        }

        public async Task Handle(Guid ownerId, long fromAccountId, long toAccountId, decimal transactionAmount, string comment)
        {
           if(fromAccountId == toAccountId)
               throw new Exception("The source and destination accounts cannot be the same");
            
            AccountDTO fromAccount = await _accountsService.GetAccountById(fromAccountId);
            AccountDTO toAccount = await _accountsService.GetAccountById(toAccountId);

            if(!fromAccount.Owner_id.Equals(ownerId))
                throw new Exception("The source account is not yours!");
            if (fromAccount.closed || toAccount.closed)
                throw new Exception("Cannot make transactions with closed accounts!");

            if(fromAccount.Amount - transactionAmount >= 0)
                await _accountsService.MakeTransaction(fromAccountId, toAccountId, transactionAmount, comment);
            else
                throw new Exception("Account balance is not enough to make this transaction!");
        }
    }
}