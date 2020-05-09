using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using BackendHomework.BusinessLogic.Auth;
using BackendHomework.Models;
using BackendHomework.TokenApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BackendHomework.Controllers
{

    public class AuthController : Controller
    {
        private readonly SignUpRequestHandler _signUpRequestHandler;
        private readonly LogInRequestHandler _logInRequestHandler;

        public AuthController(
            SignUpRequestHandler signUpRequestHandler, 
            LogInRequestHandler logInRequestHandler
            )
        {
            _signUpRequestHandler = signUpRequestHandler;
            _logInRequestHandler = logInRequestHandler;
        }
        
        [HttpPost("/auth/signup")]
        public async Task<IActionResult> SignUp([FromBody] SignupCredentials signupCredentials)
        {
            return GenerateToken(await _signUpRequestHandler.Handle(signupCredentials));
        }
        
        [HttpPost("/auth/login")]
        public async Task<IActionResult> LogIn([FromBody] LoginCredentials loginCredentials)
        {
            return GenerateToken(await _logInRequestHandler.Handle(loginCredentials));
        }
        public IActionResult GenerateToken(User user)
        {
            var identity = CreateIdentity(user);
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                expiration = jwt.ValidTo
            };
            return Json(response);
        }
        
        private ClaimsIdentity CreateIdentity(User user)
        {
            
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Status)
                };
            
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimTypes.NameIdentifier,
                        ClaimTypes.Role);
                return claimsIdentity;
            
            
        }
       
    }
}