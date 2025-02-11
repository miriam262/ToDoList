using TodoApi.models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.IO;
using System.Text.Json;


namespace TodoApi.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ToDoDbContext _dataContext;

        public AuthController(IConfiguration configuration, ToDoDbContext dataContext)
        {
            _configuration = configuration;
            _dataContext = dataContext;
        }


        [HttpPost("/api/login")]
        public async Task<IActionResult> Login([FromBody] Loginmodel model)
        {
            var user = _dataContext.Users?.FirstOrDefault(u => u.Id == model.Id && u.UserName == model.name && u.Password == model.password);
            if (user is not null)
            {
                var jwt = CreateJWT(user);
                await AddSession(user);
                return Ok(jwt);
            }
            return Unauthorized();
        }
        [HttpPost("/api/register")]
        public async Task<IActionResult> Register([FromBody] Loginmodel loginModel)
        {
            if (loginModel is not null)
            {
                var newUser = new User { Id = (int)loginModel.Id, UserName = loginModel.name, Password = loginModel.password };
                _dataContext.Users?.Add(newUser);
                await _dataContext.SaveChangesAsync();
                var jwt = CreateJWT(newUser);
                await AddSession(newUser);
                return Ok(jwt);
            }
            return (BadRequest());
        }
        private object CreateJWT(User user)
        {
            var claims = new List<Claim>()
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("name", user.UserName?? string.Empty),
                    new Claim("Password", user.Password),
                };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Key") ?? string.Empty));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JWT:Issuer"),
                audience: _configuration.GetValue<string>("JWT:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return new { Token = tokenString };
        }
        // private void AddSession(User user)
        // {
        //     try
        //     {
        //         int lastId;
        //         if (_dataContext.Sessions is not null)
        //             lastId = _dataContext.Sessions?.Max(u => u.Id) ?? 0;
        //         else
        //             lastId = 0;
        //         _dataContext.Sessions?.Add(new Session { Id = lastId + 1, UserId = user.Id, DateTime = DateTime.Now, Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString(), IsValid = true });
        //     }
        //     catch (InvalidOperationException ex)
        //     {
        //         Console.WriteLine($"Error occurred: {ex.Message}");
        //     }
        // }


        private async Task AddSession(User user)
        {
            try
            {
                int lastId;
                // if (_dataContext.Sessions is not null)
                //     lastId = _dataContext.Sessions?.Max(u => u.Id) ?? 0;
                // else
                //     lastId = 0;
                // _dataContext.Sessions?.Add(new Session { Id = lastId + 1, UserId = user.Id, DateTime = DateTime.Now, Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString(), IsValid = true });
                _dataContext.Sessions?.Add(new Session {  UserId = user.Id, DateTime = DateTime.Now, Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString(), IsValid = true });
                await _dataContext.SaveChangesAsync();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
    }
}
