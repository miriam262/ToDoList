using Microsoft.AspNetCore.Mvc;
using TodoApi.nuun;

namespace TodoApi.Controllers
{
    [AuthenticatedFilter]
    public class AuthenticatedController : ControllerBase
    {
        public required CustomIdentity Identity { get; set; }
    }
}
