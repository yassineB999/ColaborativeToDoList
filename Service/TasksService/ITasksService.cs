using CollaborativeToDoList.ViewModels.TasksViewModels.request;
using CollaborativeToDoList.ViewModels.TasksViewModels.response;

namespace CollaborativeToDoList.Service.TasksService
{
    public interface ITasksService
    {
        Task<ResponseTasksDTO> CreateTaskInTodoList(CreateTasksDTO createTasksDTO);
        Task<ResponseTasksDTO> UpdateTaskInTodoList(UpdateTasksDTO updateTasksDTO);
        Task DeleteTaskInTodoList(DeleteTasksDTO deleteTasksDTO);
        Task<IEnumerable<ResponseTasksDTO>> GetAllTasksInTodoList(GetAlLTasksDTO getAlLTasksDTO);
    }
}
