using CollaborativeToDoList.Models;

namespace CollaborativeToDoList.Repository.TodoListsRepos
{
    public interface ITodoListsRepository
    {
        Task<TodoLists>  CreateTodoList(TodoLists todoList);

        Task<TodoLists> UpdateTodoList(TodoLists todoList);

        Task<TodoLists> DeleteTodoList(int id);

        Task<TodoLists> GetTodoListById(int id);

        Task<IEnumerable<TodoLists>> GetAllTodoList();

        Task<IEnumerable<TodoLists>> GetAllMyTodoList(int userId);
    }
}
