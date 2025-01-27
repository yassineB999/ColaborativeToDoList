using CollaborativeToDoList.ViewModels.CollaboratorsViewModels.response;
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

        Task<IEnumerable<ResponseTodoListsDTO>> GetJoinedTodoLists();
        Task ApproveCollaborator(int collaboratorId);
        Task RejectCollaborator(int collaboratorId);
        Task<IEnumerable<ResponseCollaboratorDTO>> GetPendingCollaboratorsByOwnerId(int ownerId);
        Task JoinTodoListBySharedUrl(JoinTodoListDTO joinTodoListDTO);
        Task<ResponseTodoListsDTO> GetTodoListById(int id);

        Task LeaveTodoList(int todoListId);
    }
}
