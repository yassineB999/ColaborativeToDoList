using CollaborativeToDoList.Models;

namespace CollaborativeToDoList.Repository.TasksRepos
{
    public interface ITasksRepository
    {
        Task<Tasks> CreateTask(Tasks task);

        Task<Tasks> UpdateTask(Tasks task);

        Task<Tasks> DeleteTask(int id);

        Task<Tasks> GetTaskById(int id);

        Task<IEnumerable<Tasks>> GetAllTask();

        Task<Tasks> GetTasksByTodoListId(int id);


    }
}
