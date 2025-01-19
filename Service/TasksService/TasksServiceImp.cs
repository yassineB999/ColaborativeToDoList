using CollaborativeToDoList.Models;
using CollaborativeToDoList.Repository.TasksRepos;
using CollaborativeToDoList.Repository.TodoListsRepos;
using CollaborativeToDoList.Service.UserService;
using CollaborativeToDoList.ViewModels.TasksViewModels.request;
using CollaborativeToDoList.ViewModels.TasksViewModels.response;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Build.Framework;
using System.Threading.Tasks;

namespace CollaborativeToDoList.Service.TasksService
{
    public class TasksServiceImp : ITasksService
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly ITodoListsRepository _todoListsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TasksServiceImp(ITasksRepository tasksRepository, ITodoListsRepository todoListsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _tasksRepository = tasksRepository;
            _todoListsRepository = todoListsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

       async  Task<ResponseTasksDTO> ITasksService.CreateTaskInTodoList(CreateTasksDTO createTasksDTO)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int UsersId))
            {
                throw new InvalidOperationException("User ID not found in claims or invalid.");
            }

            // Validate that the to-do list exists and belongs to the authenticated user
            var todoList = await _todoListsRepository.GetTodoListById(createTasksDTO.todoListId);
            if (todoList == null)
            {
                throw new KeyNotFoundException("To-do list not found.");
            }

            if (todoList.UserId != UsersId)
            {
                throw new UnauthorizedAccessException("You do not have permission to create a task in this to-do list.");
            }

            var task = new Tasks
            {
                Description = createTasksDTO.Description,
                CreatedAt = DateTime.UtcNow,
                EndedAt = createTasksDTO.EndedAt,
                TodoListId = createTasksDTO.todoListId,
                CategoriesId = createTasksDTO.CategoriesId
            };

            var createdTask = await _tasksRepository.CreateTask(task);

            var responseDto = new ResponseTasksDTO
            (
                Id: createdTask.Id,
                Description: createdTask.Description,
                CreatedAt: createdTask.CreatedAt,
                EndedAt: createdTask.EndedAt,
                CategoryName : createdTask.Categories.Name
                );

            return responseDto;
        }

       async Task ITasksService.DeleteTaskInTodoList(DeleteTasksDTO deleteTasksDTO)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int UsersId))
            {
                throw new InvalidOperationException("User ID not found in claims or invalid.");
            }

            // Validate that the to-do list exists and belongs to the authenticated user
            var todoList = await _todoListsRepository.GetTodoListById(deleteTasksDTO.todoListId);
            if (todoList == null)
            {
                throw new KeyNotFoundException("To-do list not found.");
            }

            if (todoList.UserId != UsersId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete a task in this to-do list.");
            }

            var task = await _tasksRepository.GetTaskById(deleteTasksDTO.Id);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            await _tasksRepository.DeleteTask(deleteTasksDTO.Id);
        }

        async Task<IEnumerable<ResponseTasksDTO>> ITasksService.GetAllTasksInTodoList(GetAlLTasksDTO getAlLTasksDTO)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int UsersId))
            {
                throw new InvalidOperationException("User ID not found in claims or invalid.");
            }

            var todoList = await _todoListsRepository.GetTodoListById(getAlLTasksDTO.todoListId);
            if (todoList == null)
            {
                throw new KeyNotFoundException("To-do list not found.");
            }

            if (todoList.UserId != UsersId)
            {
                throw new UnauthorizedAccessException("You do not have permission to view a task in this to-do list.");
            }

            var getTasks = await _tasksRepository.GetTasksByTodoListId(getAlLTasksDTO.todoListId);

            var responseDto = getTasks.Select(task => new ResponseTasksDTO
            (
                Id: task.Id,
                Description: task.Description,
                CreatedAt: task.CreatedAt,
                EndedAt: task.EndedAt,
                CategoryName: task.Categories.Name
            )).ToList();


            return responseDto;

        }
       

        async Task<ResponseTasksDTO> ITasksService.UpdateTaskInTodoList(UpdateTasksDTO updateTasksDTO)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int UsersId))
            {
                throw new UnauthorizedAccessException("User ID not found in claims or invalid.");
            }

            var todoList = await _todoListsRepository.GetTodoListById(updateTasksDTO.todoListId);
            if (todoList == null)
            {
                throw new KeyNotFoundException("To-do list not found.");
            }

            if (todoList.UserId != UsersId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update task in this to-do list.");
            }

            var task = await _tasksRepository.GetTaskById(updateTasksDTO.Id);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            task.Description = updateTasksDTO.Description;
            task.EndedAt = updateTasksDTO.EndedAt;
            task.CategoriesId = updateTasksDTO.CategoriesId;

            var updatedTask = await _tasksRepository.UpdateTask(task);

            var responseDto = new ResponseTasksDTO
            (
                Id: updatedTask.Id,
                Description: updatedTask.Description,
                CreatedAt: updatedTask.CreatedAt,
                EndedAt: updatedTask.EndedAt,
                CategoryName: updatedTask.Categories.Name
                );

            return responseDto;

        }
    }
}
