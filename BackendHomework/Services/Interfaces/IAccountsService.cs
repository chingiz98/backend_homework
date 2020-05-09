using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendHomework.Models;

namespace BackendHomework.Services.Interfaces
{
    public interface IAccountsService
    {
        Task<AccountDTO> GetAccountById(long id);
        Task<List<AccountDTO>> GetAccountsByOwnerId(Guid ownerId);
        Task<List<TransactionDTO>> GetTransactionsByAccountId(long accountId);
        Task<AccountDTO> CreateAccount(string name, Guid ownerId);
        Task<AccountDTO> CloseAccount(long id, Guid ownerId);
        Task<AccountDTO> RenameAccount(long id, Guid ownerId, string name);
        Task MakeDeposit(long id, decimal amount, string comment = "");
        Task MakeTransaction(long fromId, long toId, decimal amount, string comment = "");
    }
}