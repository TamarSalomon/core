using myTask.Interfaces;
using myTask.Models;
namespace myTask.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;


public class TaskService : ITaskService
{
    List<TheTask> Tasks { get; }
    private readonly string fileName = "tasks.json";
    public TaskService(IWebHostEnvironment webHost)
    {
        fileName = Path.Combine(webHost.ContentRootPath, "Data", "tasks.json");
        using var jsonFile = File.OpenText(fileName);
        Tasks = JsonSerializer.Deserialize<List<TheTask>>(jsonFile.ReadToEnd(),
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    // Saves the tasks data to a file using JSON serialization.
    private void SaveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(Tasks));
    }

    //Retrieving all the tasks of a certain user 
    public List<TheTask> GetAll(int userId)
    {
        List<TheTask> userTasks = new List<TheTask>();
        userTasks = Tasks.Where(t => t.UserId == userId).ToList();
        return userTasks;
    }

    // Retrieving a certain task of a certain user
    public TheTask? Get(int taskId, int userId)
    {
        TheTask task = Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null && task.UserId == userId)
            return task;
        return null;
    }

    //Adding a specific task to a specific user
    public void Add(int userId, TheTask newTask)
    {
        newTask.UserId = userId;
        newTask.Id = GetNextId();
        Tasks.Add(newTask);
        SaveToFile();
    }


    //Updating a task
    public void Update(TheTask task)
    {
        var index = Tasks.FindIndex(t => t.Id == task.Id);
        if (index == -1)
            return;

        Tasks[index] = task;
        SaveToFile();
    }

    //Deleting a task
    public void Delete(int taskId, int userId)
    {
        TheTask task = Get(taskId, userId);
        if (task is null)
            return;

        Tasks.Remove(task);
        SaveToFile();
    }

    //Deleting all tasks of a certain user
    public void DeleteByUserId(int userId)
    {
        Tasks.RemoveAll(task => task.UserId == userId);
    }

    //Returning the id
    public int GetNextId() => Tasks.Max(task => task.Id) + 1;

}

// Extension method to register a TaskService implementation as a singleton in the IServiceCollection
public static class TaskUtils
{
    public static void AddTask(this IServiceCollection service)
    {
        service.AddSingleton<ITaskService, TaskService>();
    }
}
