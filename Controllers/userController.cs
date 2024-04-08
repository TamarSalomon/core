using Microsoft.AspNetCore.Mvc;
using myTask.Models;
using myTask.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace myTask.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        readonly IUserService userService;
        readonly ITaskService taskService;
        readonly int UserId;
        public UserController(IUserService userService, ITaskService taskService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.taskService = taskService;
            UserId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value, CultureInfo.InvariantCulture);
        }

        //Get all users
        [HttpGet("/api/Allusers")]
        [Authorize(Policy = "Admin")]
        public ActionResult<List<User>> GetAll() =>
            userService.GetAll();

        //Get my user
        [HttpGet]
        [Authorize(Policy = "User")]
        public ActionResult<User> Get()
        {
            var user = userService.Get(UserId);
            if (user == null)
                return NotFound();
            return user;
        }

        //Add a new user 
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(User user)
        {
            if (user is null)
                return BadRequest("user is null");
            userService.Add(user);
            return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
        }

        //Update the user
        [HttpPut]
        [Authorize(Policy = "User")]
        public IActionResult Update([FromBody] User user)
        {
            var existingUser = userService.Get(user.Id);
            if (existingUser is null)
                return NotFound();
            userService.Update(user);
            return NoContent();
        }

        //Delete user and all his to-do's
        [HttpDelete]
        [Route("{userId}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int userId)
        {
            var user = userService.Get(userId);
            if (user is null)
                return NotFound();

            userService.Delete(userId);
            taskService.DeleteByUserId(userId);

            return NoContent();
        }

        //Check if it is a manager
        [HttpGet]
        [Route("/Admin")]
        [Authorize(Policy = "Admin")]
        public ActionResult<string> IsAdmin()
        {
            return new OkObjectResult("true");
        }

    }

}

