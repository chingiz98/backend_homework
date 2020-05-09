using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackendHomework.Controllers.Attributes
{
    public class RightsAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //в будущем тут будет проверка на права. Если status = "moderation", у юзера временно не будет доступа к методам API
            if (context.HttpContext.User.FindFirst(ClaimTypes.Role).Value.Equals("moderation")) 
            {
                //throw new Exception("Your profile is under moderation!");
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("action executed");
        }
    }
}