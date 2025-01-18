using CollaborativeToDoList.Data;
using CollaborativeToDoList.Models;

namespace CollaborativeToDoList.Repository.TasksRepos
{
    public class TasksRepositoryImp : ITasksRepository
    {

        private readonly TodoListDbContext _db;

        public TasksRepositoryImp(TodoListDbContext db)
        {
            _db = db;
        }
        public Task<Tasks> CreateTask(Tasks task)
        {
            throw new NotImplementedException();
        }

        public Task<Tasks> DeleteTask(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tasks>> GetAllTask()
        {
            throw new NotImplementedException();
        }

        public Task<Tasks> GetTaskById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Tasks> UpdateTask(Tasks task)
        {
            throw new NotImplementedException();
        }
    }
}
