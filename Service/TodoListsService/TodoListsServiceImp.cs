using CollaborativeToDoList.Data;
using CollaborativeToDoList.Models;
using CollaborativeToDoList.Repository.TodoListsRepos;
using CollaborativeToDoList.Repository.UsersRepos;
using CollaborativeToDoList.ViewModels.TodoListViewModels.request;
using CollaborativeToDoList.ViewModels.TodoListViewModels.response;

namespace CollaborativeToDoList.Service.TodoListsService
{
    public class TodoListsServiceImp : ITodoListsService
    {
        private readonly ITodoListsRepository _todoListsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TodoListsServiceImp(ITodoListsRepository todoListsRepository, IUsersRepository usersRepository, IHttpContextAccessor httpContextAccessor)
        {
            _todoListsRepository = todoListsRepository;
            _usersRepository = usersRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseTodoListsDTO> CreateTodoList(CreateTodoListsDTO createTodoListsDTO)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int UsersId))
            {
                throw new InvalidOperationException("User ID not found in claims or invalid.");
            }

            var todoList = new TodoLists
            {
                Title = createTodoListsDTO.Title,
                UserId = UsersId,
                SharedUrl = null 
            };

            var createdTodoList = await _todoListsRepository.CreateTodoList(todoList);

            var owner = await _usersRepository.GetUserById(UsersId);
            if (owner == null)
            {
                throw new InvalidOperationException("Owner user not found.");
            }

            var responseDto = new ResponseTodoListsDTO(
                Id: createdTodoList.Id,
                Title: createdTodoList.Title,
                OwnerId: owner.Id,
                OwnerName: owner.FullName
            );

            return responseDto;
        }

        public async Task DeleteTodoList(DeletetodoListsDTO deletetodoListsDTO)
        {
            var todoList = await _todoListsRepository.GetTodoListById(deletetodoListsDTO.Id);

            if (todoList == null)
            {
                throw new ArgumentException("Todo list not found.");
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                throw new InvalidOperationException("User ID not found in claims or invalid.");
            }

            if (todoList.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this Todo list.");
            }

            await _todoListsRepository.DeleteTodoList(todoList.Id);
        }

        public async Task<IEnumerable<ResponseTodoListsDTO>> GetAllMyTodoLists()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }

            var myTodoLists = await _todoListsRepository.GetAllMyTodoList(currentUserId);

            var owner = await _usersRepository.GetUserById(currentUserId);
            if (owner == null)
            {
                throw new InvalidOperationException("Owner user not found.");
            }

            var myData = myTodoLists
                .Select(item => new ResponseTodoListsDTO(
                    Id: item.Id,
                    Title: item.Title,
                    OwnerId: owner.Id,
                    OwnerName: owner.FullName
                ))
                .ToList();

            return myData;
        }

        public async Task<ResponseTodoListsDTO> GetTodoListById(int id)
        {
            var todoList = await _todoListsRepository.GetTodoListById(id);
            if (todoList == null)
            {
                throw new KeyNotFoundException("Todo list not found.");
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }

            if (todoList.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this to-do list.");
            }

            var owner = await _usersRepository.GetUserById(todoList.UserId);
            if (owner == null)
            {
                throw new InvalidOperationException("Owner user not found.");
            }

            var responseDto = new ResponseTodoListsDTO(
                Id: todoList.Id,
                Title: todoList.Title,
                OwnerId: owner.Id,
                OwnerName: owner.FullName
            );

            return responseDto;
        }

        public async Task<ResponseTodoListsDTO> UpdateTodoList(UpdateTodoListDTO updateTodoListDTO)
        {
            var todoList = await _todoListsRepository.GetTodoListById(updateTodoListDTO.Id);
            if (todoList == null)
            {
                throw new KeyNotFoundException("Todo list not found.");
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }

            if (todoList.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this to-do list.");
            }

            todoList.Title = updateTodoListDTO.Title;

            await _todoListsRepository.UpdateTodoList(todoList);

            var owner = await _usersRepository.GetUserById(todoList.UserId);
            if (owner == null)
            {
                throw new InvalidOperationException("Owner user not found.");
            }

            var responseDto = new ResponseTodoListsDTO(
                Id: todoList.Id,
                Title: todoList.Title,
                OwnerId: owner.Id,
                OwnerName: owner.FullName
            );

            return responseDto;
        }
    }
}
