using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Principal;
using TodoApi;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrivateController : AuthenticatedController
    {
        private readonly ToDoDbContext _dataContext;
        private readonly ILogger<PrivateController> _logger;
        public PrivateController(ToDoDbContext dataContext, ILogger<PrivateController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Session>>> Get()
        {
            _logger.LogInformation("Entering Get method");
            // ודא שה-Identity כבר אוחסן ב-AuthenticatedController על ידי הפילטר
            var userId = Identity?.Id;  // השתמש ב-Id של המשתמש מתוך Identity
            _logger.LogWarning("No sessions found for User ID: {UserId}", userId);
            if (!userId.HasValue)
            {
                _logger.LogWarning("User ID not found. Returning Unauthorized.");
                return Unauthorized();
            }
            var sessions = await _dataContext.Sessions
                .Where(s => s.UserId == userId.Value)
                .OrderByDescending(s => s.DateTime)
                .ToListAsync();
          
            return Ok(sessions);  // החזרת הסשנים
        }

        // [HttpGet]
        // public ActionResult<IEnumerable<Session>> Get()
        // {
        //     var userId = User?.FindFirst("Id")?.Value;
        //     var d = _dataContext.Sessions?.Where(s => s.UserId.ToString().Equals(userId)).OrderByDescending(s => s.DateTime);
        //     return Ok(_dataContext.Sessions);
        //     //Console.WriteLine("I am in Private....Get()");
        //     //var userId = Identity?.Id;
        //     //Console.WriteLine(Identity);
        //     //var session = _dataContext.Sessions?
        //     //.Where(x => x.UserId == userId)
        //     //.OrderByDescending(x => x.DateTime);

        //     //if (session is not null)
        //     //{
        //     //    return Ok(session.FirstOrDefault());
        //     //}
        //     //Console.WriteLine("oijhnjuhgbhjuhgbhj");
        //     //return NotFound(Identity);

        // }
    }
}