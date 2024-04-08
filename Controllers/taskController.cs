using Microsoft.AspNetCore.Mvc;
using myTask.Models;
using myTask.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace myTask.Controllers
{
    [ApiController]
    [Route("/api/todo")]
    public class TaskController : ControllerBase
    {
        readonly ITaskService TaskService;
        readonly int UserId;
        public TaskController(ITaskService TaskService, IHttpContextAccessor httpContextAccessor)
        {
            this.TaskService = TaskService;
            UserId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value, CultureInfo.InvariantCulture);
        }

        //Get all user's to-do items 
        [HttpGet]
        [Authorize(Policy = "User")]
        public ActionResult<List<TheTask>> GetAll()
        {
            var tasks = TaskService.GetAll(UserId);
            if (tasks == null)
            {
                return NotFound();
            }
            return tasks;
        }

        //Get a user's to-do item by ID     
        [HttpGet]
        [Route("{taskId}")]
        [Authorize(Policy = "User")]
        public ActionResult<TheTask> Get(int taskId)
        {
            var task = TaskService.Get(taskId, UserId);
            if (task == null)
                return NotFound();
            return task;
        }

        //Add a new to-do item to user
        [HttpPost]
        [Authorize(Policy = "User")]
        public IActionResult Create(TheTask task)
        {
            TaskService.Add(UserId, task);
            return CreatedAtAction(nameof(Create), new { id = task.Id }, task);
        }

        //Update user's to-do item 
        [HttpPut]
        [Route("{taskId}")]
        [Authorize(Policy = "User")]
        public IActionResult Update(int taskId, TheTask task)
        {
            if (taskId != task.Id)
                return BadRequest();

            var existingTask = TaskService.Get(taskId, UserId);
            if (existingTask is null)
                return NotFound();

            TaskService.Update(task);
            return NoContent();
        }

        //Delete user's to-do item
        [HttpDelete]
        [Route("{taskId}")]
        [Authorize(Policy = "User")]
        public IActionResult Delete(int taskId)
        {
            var task = TaskService.Get(taskId, UserId);
            if (task is null)
                return NotFound();

            TaskService.Delete(taskId, UserId);
            return NoContent();
        }
    
    }
}