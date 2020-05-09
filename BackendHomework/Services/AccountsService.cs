using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendHomework.Models;
using BackendHomework.Services.Interfaces;
using Dapper;
using Npgsql;

namespace BackendHomework.Services
{
    public class AccountsService : IAccountsService
    {
        public async Task<AccountDTO> GetAccountById(long accountId)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();

                try
                {
                    string sqlQuery = "SELECT * FROM accounts WHERE id = @id";
                    return  await connection.QuerySingleAsync<AccountDTO>(sqlQuery, new
                    {
                        id = accountId
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("Incorrect account");
                }
                
            }
        }
        public async Task<List<AccountDTO>> GetAccountsByOwnerId(Guid ownerId)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();

                try
                {
                    string sqlQuery = "SELECT * FROM accounts WHERE owner_id = @ownerId";
                    return (await connection.QueryAsync<AccountDTO>(sqlQuery, new
                    {
                        ownerId = ownerId
                    })).ToList();
                } 
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("Error while getting accounts!");
                }
                
            }
        }
        public async Task<List<TransactionDTO>> GetTransactionsByAccountId(long accountId)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();

                try
                {
                    string sqlQuery = "SELECT * FROM transactions WHERE from_id = @accountId OR to_id = @accountId";
                    return (await connection.QueryAsync<TransactionDTO>(sqlQuery, new
                    {
                        accountId
                    })).ToList();
                } 
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("Error while getting accounts!");
                }
                
            }
        }
        public async Task<AccountDTO> CreateAccount(string accountName, Guid ownerId)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                try
                {
                    string sqlQuery = "INSERT INTO accounts (owner_id, name) VALUES (@owner_id, @name) RETURNING *";
                    AccountDTO dto = await connection.QuerySingleAsync<AccountDTO>(sqlQuery, new
                    {
                        owner_id = ownerId,
                        name = accountName
                    });

                    return dto;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("Error while creating account!");
                }
            }

        }
        public async Task<AccountDTO> CloseAccount(long id, Guid ownerId)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                try
                {
                    string sqlQuery =
                        "UPDATE accounts SET closed = true WHERE id = @id AND owner_id = @ownerId AND closed != true RETURNING *";
                    return await connection.QuerySingleAsync<AccountDTO>(sqlQuery, new
                    {
                        id = id,
                        ownerId = ownerId
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("Error while closing account! Probably it is already closed.");
                }
            }
        }
        public async Task<AccountDTO> RenameAccount(long id, Guid ownerId, string name)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                try
                {
                    string sqlQuery =
                        "UPDATE accounts SET name = @name WHERE id = @id AND owner_id = @ownerId AND closed != true RETURNING *";
                    return await connection.QuerySingleAsync<AccountDTO>(sqlQuery, new
                    {
                        id = id,
                        ownerId = ownerId,
                        name = name
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("Error while renaming account!");
                }
            }
        }
        public async Task MakeDeposit(long id, decimal amount, string comment = "")
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                try
                {
                    string sqlQuery = 
                        @"BEGIN TRANSACTION; 
                    UPDATE accounts SET amount = amount + @amount WHERE id = @id AND closed != true;
                    INSERT INTO transactions (to_id, from_id, amount, type, comment, timestamp) VALUES (@id, NULL, @amount, @type, @comment, @timestamp);
                    COMMIT;";
                    await connection.ExecuteAsync(sqlQuery, new
                    {
                        id = id,
                        amount = amount,
                        type = "deposit",
                        timestamp = DateTime.Now,
                        comment = comment
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("Error while making deposit!");
                }
            }
        }
        public async Task MakeTransaction(long fromId, long toId, decimal amount, string comment = "")
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                try
                {
                    string sqlQuery =
                        @"BEGIN TRANSACTION; 
                        UPDATE accounts SET amount = amount - @amount WHERE id = @fromId AND closed != true;
                        UPDATE accounts SET amount = amount + @amount WHERE id = @toId AND closed != true;
                        INSERT INTO transactions (to_id, from_id, amount, type, comment, timestamp) VALUES (@fromId, @toId, @amount, @type, @comment, @timestamp);
                        COMMIT;";
                    await connection.ExecuteAsync(sqlQuery, new
                    {
                        fromId = fromId,
                        toId = toId,
                        amount = amount,
                        type = "transaction",
                        timestamp = DateTime.Now,
                        comment = comment
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("Error while making transaction!");
                }
            }
        }
        private NpgsqlConnection CreateConnection()
        {
            var connection = new NpgsqlConnection($"server=localhost;database=test;userid=postgres;password=1;Pooling=false");

            return connection;
        }
    }
}