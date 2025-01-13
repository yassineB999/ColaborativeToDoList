using CollaborativeToDoList.ViewModels.TodoListViewModels.request;
using CollaborativeToDoList.ViewModels.TodoListViewModels.response;

namespace CollaborativeToDoList.Service.TodoListsService
{
    public interface ITodoListsService
    {
        Task<ResponseTodoListsDTO> CreateTodoList(CreateTodoListsDTO createTodoListsDTO);
        Task<ResponseTodoListsDTO> UpdateTodoList(UpdateTodoListDTO updateTodoListDTO);
        Task DeleteTodoList(DeletetodoListsDTO deletetodoListsDTO);
        Task<IEnumerable<ResponseTodoListsDTO>> GetAllMyTodoLists();

        Task<ResponseTodoListsDTO> GetTodoListById(int id);
    }
}
