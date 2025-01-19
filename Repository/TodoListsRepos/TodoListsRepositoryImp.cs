using CollaborativeToDoList.Data;
using CollaborativeToDoList.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace CollaborativeToDoList.Repository.TodoListsRepos
{
    public class TodoListsRepositoryImp : ITodoListsRepository
    {

        private readonly TodoListDbContext _db;

        public TodoListsRepositoryImp(TodoListDbContext db)
        {
            _db = db;
        }

        public async Task<TodoLists> CreateTodoList(TodoLists todoList)
        {
            _db.Add(todoList);
            await _db.SaveChangesAsync();
            return todoList;
        }

        public async Task<TodoLists> DeleteTodoList(int id)
        {
            var todoList = await _db.TodoLists.FindAsync(id);
            if (todoList == null)
            {
                return null;
            }

            _db.TodoLists.Remove(todoList);
            await _db.SaveChangesAsync();
            return todoList;
        }

        public async Task<IEnumerable<TodoLists>> GetAllMyTodoList(int userId)
        {
             return await _db.TodoLists
            .Where(tl => tl.UserId == userId)
            .ToListAsync();
        }

        public async Task<IEnumerable<TodoLists>> GetAllTodoList()
        {
            return await _db.TodoLists.ToListAsync();
        }

        public async Task<TodoLists> GetTodoListById(int id)
        {
            return await _db.TodoLists.FindAsync(id);
        }

        public async Task<TodoLists> UpdateTodoList(TodoLists todoList)
        {
            _db.Entry(todoList).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return todoList;
        }
    }
}
