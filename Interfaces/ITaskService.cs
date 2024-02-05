using myTask.Models;
using System.Collections.Generic;

namespace myTask.Interfaces
{
    public interface ITaskService
    {
        List<theTask> GetAll();
        theTask Get(int id);
        void Add(theTask myTask);
        void Delete(int id);
        void Update(theTask myTask);
        int Count {get;}
    }
}