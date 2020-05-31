using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BackendHomework.BusinessLogic.Accounts;
using BackendHomework.Controllers.Attributes;
using BackendHomework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendHomework.Controllers
{
    [Authorize]
    [Rights]
    public class AccountsController : Controller
    {
        private readonly CreateAccountRequestHandler _createAccountRequestHandler;
        private readonly CloseAccountRequestHandler _closeAccountRequestHandler;
        private readonly MakeDepositRequestHandler _makeDepositRequestHandler;
        private readonly GetAccountsRequestHandler _getAccountsRequestHandler;
        private readonly MakeTransactionRequestHandler _makeTransactionRequestHandler;
        private readonly RenameAccountRequestHandler _renameAccountRequestHandler;
        private readonly GetTransactionsRequestHandler _getTransactionsRequestHandler;
        
        
        public AccountsController(
            CreateAccountRequestHandler createAccountRequestHandler, 
            CloseAccountRequestHandler closeAccountRequestHandler, 
            MakeDepositRequestHandler makeDepositRequestHandler, 
            GetAccountsRequestHandler getAccountsRequestHandler, 
            MakeTransactionRequestHandler makeTransactionRequestHandler, 
            RenameAccountRequestHandler renameAccountRequestHandler, 
            GetTransactionsRequestHandler getTransactionsRequestHandler
            )
        {
            _createAccountRequestHandler = createAccountRequestHandler;
            _closeAccountRequestHandler = closeAccountRequestHandler;
            _makeDepositRequestHandler = makeDepositRequestHandler;
            _getAccountsRequestHandler = getAccountsRequestHandler;
            _makeTransactionRequestHandler = makeTransactionRequestHandler;
            _renameAccountRequestHandler = renameAccountRequestHandler;
            _getTransactionsRequestHandler = getTransactionsRequestHandler;
        }
        
        [HttpGet("/accounts/getAccounts")]
        public async Task<List<AccountDTO>> GetAccounts()
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            return await _getAccountsRequestHandler.HandleGetAccounts(ownerId);
        }
        
        [HttpGet("/accounts/getAccountById")]
        public async Task<AccountDTO> GetAccountById(long id)
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            return await _getAccountsRequestHandler.HandleGetAccountById(ownerId, id);
        }
        
        [HttpGet("/accounts/getTransactions")]
        public async Task<List<TransactionDTO>> GetTransactions(long accountId)
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            
            
            return await _getTransactionsRequestHandler.HandleGet(accountId, ownerId);
        }
        
        [HttpGet("/accounts/getAllTransactions")]
        public async Task<List<TransactionDTO>> GetAllTransactions()
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            
            return await _getTransactionsRequestHandler.HandleGetAll(ownerId);
        }
        
        [HttpPost("/accounts/makeTransaction")]
        public async Task<IActionResult> MakeTransaction(
            long fromAccountId,
            long toAccountId,
            decimal amount,
            string comment = "")
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            await _makeTransactionRequestHandler.Handle(ownerId, fromAccountId, toAccountId, amount, comment);
            return Json(new
            {
                result = "ok"
            });
        }

        [HttpPost("/accounts/rename")]
        public Task<IActionResult> Rename(long accountId, string name)
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            _renameAccountRequestHandler.Handle(accountId, ownerId, name);
            return Task.FromResult<IActionResult>(Json(new
            {
                result = "ok"
            }));
        }

        [HttpPost("/accounts/create")]
        public async Task<AccountDTO> Create(string name)
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            return await _createAccountRequestHandler.Handle(name, ownerId);
        }
        
        [HttpPost("/accounts/close")]
        public async Task<AccountDTO> Close(long id)
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            return await _closeAccountRequestHandler.Handle(id, ownerId);
        }
        
        [HttpPost("/accounts/deposit")]
        public Task<IActionResult> Deposit(long id, decimal amount)
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            _makeDepositRequestHandler.Handle(id, ownerId, amount);
            return Task.FromResult<IActionResult>(Json(new
            {
                result = "ok"
            }));
        }
        
    }
}