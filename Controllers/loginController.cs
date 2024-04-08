
using Microsoft.AspNetCore.Mvc;
using myTask.Models;
using myTask.Interfaces;
using myTask.Services;
using System.Security.Claims;

namespace myTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        readonly IUserService userService;
        public LoginController(IUserService userService)
        {
            this.userService = userService;
        }

        //Login
        [HttpPost("/api/login")]
        public ActionResult<string> Login([FromBody] User user)
        {
            User myUser = userService.GetAll().FirstOrDefault(u => u.Name == user.Name && u.Password == user.Password);
            if (myUser == null)
                return Unauthorized();
            var claims = new List<Claim>
            {
                new("Type","User"),
                new("Id",myUser.Id.ToString())
            };

            if (myUser.Type == "admin")
                claims.Add(new Claim("Type", "Admin"));
            var token = TokenService.GetToken(claims);

            return new OkObjectResult(TokenService.WriteToken(token));
        }
    }
}

