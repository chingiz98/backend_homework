using System;
using System.Threading.Tasks;
using BackendHomework.Models;
using BackendHomework.BusinessLogic.Auth;

namespace BackendHomework.Services.Interfaces
{
    public interface IUserService
    {
        Task AddNewUser(UserDto auth);
        Task<UserDto> GetUser(string username);
        Task CloseUserProfile(Guid id);
        Task UpdateUserInfo(Guid id, string username, string name);
    }
}