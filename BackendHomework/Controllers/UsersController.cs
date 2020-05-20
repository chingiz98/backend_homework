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
        private readonly ProfileRequestsHandler _profileRequestsHandler;
        private readonly UpdateUserInfoRequestHandler _updateUserInfoRequestHandler;
        public UsersController(
            ProfileRequestsHandler profileRequestsHandler, 
            UpdateUserInfoRequestHandler updateUserInfoRequestHandler
            )
        {
            _profileRequestsHandler = profileRequestsHandler;
            _updateUserInfoRequestHandler = updateUserInfoRequestHandler;
        }
        [HttpDelete("/user/deleteProfile")]
        public Task<IActionResult> DeleteProfile()
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            _profileRequestsHandler.HandleDelete(ownerId);
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
        
        [HttpGet("/user/getInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            Guid ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                      throw new Exception("Invalid token"));
            var result = await _profileRequestsHandler.HandleGetInfo(ownerId);
            return Json(new
            {
                id = result.Id,
                name = result.Name,
                username = result.Username,
                status = result.Status
            });
        }
    }
}