using CollaborativeToDoList.Data;
using CollaborativeToDoList.Models;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeToDoList.Repository.TasksRepos
{
    public class TasksRepositoryImp : ITasksRepository
    {

        private readonly TodoListDbContext _db;

        public TasksRepositoryImp(TodoListDbContext db)
        {
            _db = db;
        }
        public async Task<Tasks> CreateTask(Tasks task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
            return task;
        }

        public async Task<Tasks> DeleteTask(int id)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
            return task;
        }

        public async Task<Tasks> GetTaskById(int id)
        {
            var task = await _db.Tasks
                 .Include(t => t.TodoLists) 
                 .Include(t => t.Categories) 
                 .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            return task;
        }

        public async Task<IEnumerable<Tasks>> GetTasksByTodoListId(int todoListId)
        {
            var tasks = await _db.Tasks
             .Include(t => t.Categories) 
             .Where(t => t.TodoListId == todoListId)
             .ToListAsync();
            return tasks;
        }

        public async Task<Tasks> UpdateTask(Tasks task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var existingTask = await _db.Tasks.FindAsync(task.Id);
            if (existingTask == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            existingTask.Description = task.Description;
            existingTask.EndedAt = task.EndedAt;
            existingTask.CategoriesId = task.CategoriesId;

            _db.Tasks.Update(existingTask);
            await _db.SaveChangesAsync();
            return existingTask;
        }
    }
}
