using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BackendHomework.BusinessLogic.User;
using BackendHomework.Controllers.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendHomework.Controllers
{
    [Authorize]
    [Rights]
    public class UsersController : Controller
    {
        private readonly DeleteProfileRequestHandler _deleteProfileRequestHandler;
        private readonly UpdateUserInfoRequestHandler _updateUserInfoRequestHandler;
        public UsersController(
            DeleteProfileRequestHandler deleteProfileRequestHandler, 
            UpdateUserInfoRequestHandler updateUserInfoRequestHandler
            )
        {
            _deleteProfileRequestHandler = deleteProfileRequestHandler;
            _updateUserInfoRequestHandler = updateUserInfoRequestHandler;
        }
        [HttpDelete("/user/deleteProfile")]
        public Task<IActionResult> DeleteProfile()
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            _deleteProfileRequestHandler.Handle(ownerId);
            return Task.FromResult<IActionResult>(Json(new
            {
                result = "ok"
            }));
        }
        [HttpPost("/user/updateInfo")]
        public Task<IActionResult> UpdateInfo(string username, string name)
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            _updateUserInfoRequestHandler.Handle(ownerId, username, name);
            return Task.FromResult<IActionResult>(Json(new
            {
                result = "ok"
            }));
        }
    }
}