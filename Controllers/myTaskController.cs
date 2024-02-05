using Microsoft.AspNetCore.Mvc;
using myTask.Models;
using myTask.Interfaces;

namespace myTask.Controllers;

[ApiController]
[Route("[controller]")]
public class myTaskController : ControllerBase
{
    ITaskService TaskService;
    public myTaskController(ITaskService TaskService)
    {
        this.TaskService = TaskService;
    }
    [HttpGet]
    public ActionResult<List<theTask>> GetAll()=>
        TaskService.GetAll();
    

    [HttpGet("{id}")]
    public ActionResult<theTask> Get(int id)
    {
        var task = TaskService.Get(id);
        if (task == null)
            return NotFound();
        return task;
    }

    [HttpPost]
    public IActionResult Create(theTask task)
    {
        TaskService.Add(task);
        return CreatedAtAction(nameof(Create), new {id=task.Id}, task);

    }
    
    [HttpPut("{id}")]
    public IActionResult Update(int id, theTask task)
        {
            if (id != task.Id)
                return BadRequest();

            var existingTask = TaskService.Get(id);
            if (existingTask is null)
                return  NotFound();

            TaskService.Update(task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = TaskService.Get(id);
            if (task is null)
                return  NotFound();

            TaskService.Delete(id);

            return Content(TaskService.Count.ToString());
        }
    
}
