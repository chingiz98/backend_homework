using System;
using System.Threading.Tasks;
using BackendHomework.Models;
using BackendHomework.Services.Interfaces;
using BackendHomework.BusinessLogic.Auth;
using Dapper;
using Npgsql;

namespace BackendHomework.Services
{
    public class UserService : IUserService
    {
        public async Task AddNewUser(UserDto user)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                try
                {
                    var sqlQuery =
                        "INSERT INTO users (id, username, password, name, status) VALUES (@id, @username, @password, @name, @status);";
                    await connection.ExecuteAsync(sqlQuery, user);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("This username is already taken!");
                }
            }
        }
        public async Task<UserDto> GetUser(string username)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                try
                {
                    string sqlQuery = @"SELECT * FROM users WHERE username = @username";
                    return await connection.QuerySingleAsync<UserDto>(sqlQuery, new {username = username});
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("Invalid username!");
                }
            }
        }
        public async Task CloseUserProfile(Guid id)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                try
                {
                    string sqlQuery =
                        @"BEGIN TRANSACTION; 
                        UPDATE users SET status = @status WHERE id = @id;
                        UPDATE accounts SET closed = @accClosed WHERE owner_id = @id;
                        COMMIT;";
                    await connection.ExecuteAsync(sqlQuery, new
                    {
                        status = "deleted",
                        accClosed = true,
                        id
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("Error while deleting account!");
                }
            }
        }
        public async Task UpdateUserInfo(Guid id, string username, string name)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                try
                {
                    string sqlQuery =
                        "UPDATE users SET username = COALESCE(@username, username), name = COALESCE(@name, name) WHERE id = @id;";
                    await connection.ExecuteAsync(sqlQuery, new
                    {
                        username,
                        name,
                        id
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception("Error while updating user data!");
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