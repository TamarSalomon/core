using myTask.Interfaces;
using myTask.Models;
namespace myTask.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using System;

public class UserService : IUserService
{
    List<User> Users { get; }
    private readonly string fileName = "users.json";
    public UserService(IWebHostEnvironment webHost)
    {
        fileName = Path.Combine(webHost.ContentRootPath, "Data", "users.json");
        using var jsonFile = File.OpenText(fileName);
        
        Users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    // Saves the users data to a file using JSON serialization.
    private void SaveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(Users));
    }

    //Retrieving all existing users
    public List<User> GetAll() => Users;

    //Retrieving user by id
    public User Get(int userId)
    {
        return Users.FirstOrDefault(u => u.Id == userId);
    }

    //Adding user
    public void Add(User newUser)
    {
        newUser.Id = GetNextId();
        Users.Add(newUser);
        SaveToFile();
    }

    //Updating a user
    public void Update(User user)
    {
        var index = Users.FindIndex(u => u.Id == user.Id);
        if (index == -1)
            return;
        user.Type=Users[index].Type;
        user.Id=Users[index].Id;
        Users[index] = user;
        SaveToFile();
    }

    //Deleting a user by id
    public void Delete(int userId)
    {
        var user = Get(userId);
        if (user is null)
            return;

        Users.Remove(user);
        SaveToFile();
    }

    //Returning the id
    public int GetNextId() => Users.Max(user => user.Id) + 1;

    public object GetToken(List<Claim> claims)
    {
        throw new NotImplementedException();
    }

}

// Extension method to add a UserService implementation as a singleton to the IServiceCollection
public static class UserUtils
{
    public static void AddUser(this IServiceCollection service)
    {
        service.AddSingleton<IUserService, UserService>();
    }
}


