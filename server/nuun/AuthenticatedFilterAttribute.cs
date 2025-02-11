using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoApi.Controllers;

namespace TodoApi.nuun
{
    public class AuthenticatedFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = (AuthenticatedController)context.Controller;
            controller.Identity = context.HttpContext.User.Identities.OfType<CustomIdentity>().FirstOrDefault();

        }
    }
}
