using myTask.Interfaces;
using myTask.Models;
namespace myTask.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Diagnostics;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class theTaskService : ITaskService
{
    List<theTask> tasks {get;}
    
    private string fileName ="task.jsom";
    public theTaskService(IWebHostEnvironment  webHost)
    {
        this.fileName =Path.Combine(webHost.ContentRootPath, "task.json");
        using (var jsonFile = File.OpenText(fileName))
        {
            tasks = JsonSerializer.Deserialize<List<theTask>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
           
            
        }
       
    }
        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(tasks));
        }
    public  List<theTask> GetAll() => tasks;

    public  theTask Get(int id) 
    {
        return tasks.FirstOrDefault(p => p.Id == id);
    }

    public void Add(theTask newTask)
    {
        newTask.Id = tasks.Count()+1;
        tasks.Add(newTask);
        saveToFile();
    }
  
    public void Delete(int id)
    {
        var task = Get(id);
        if (task is null)
            return;

        tasks.Remove(task);
        saveToFile();
    }

    public void Update(theTask task)
        {
            var index = tasks.FindIndex(p => p.Id == task.Id);
            if (index == -1)
                return;

            tasks[index] = task;
            saveToFile();
        }
        public int Count => tasks.Count();

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}