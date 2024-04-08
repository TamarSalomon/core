using myTask.Models;

namespace myTask.Interfaces
{
    public interface ITaskService
    {
        List<TheTask> GetAll(int userId);
        TheTask Get(int taskId,int userId);
        void Add(int userId,TheTask myTask);
        void Update(TheTask myTask);
        void Delete(int taskId,int userId);
        void DeleteByUserId(int userId);
    }
}