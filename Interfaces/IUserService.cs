using myTask.Models;

namespace myTask.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
        User Get(int userId);
        void Add(User user); 
        void Update(User user);
        void Delete(int id);
    }
}
