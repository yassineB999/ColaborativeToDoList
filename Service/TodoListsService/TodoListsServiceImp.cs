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

            // Create a new TodoLists entity
            var todoList = new TodoLists
            {
                Title = createTodoListsDTO.Title,
                UsersId = UsersId,
                SharedUrl = ""
            };

            await _todoListsRepository.CreateTodoList(todoList);

            var owner = await _usersRepository.GetUserById(UsersId);

            if (owner == null)
            {
                throw new InvalidOperationException("Owner user not found.");
            }

            var responseDto = new ResponseTodoListsDTO(todoList.Id, todoList.Title, owner);

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
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int UsersId))
            {
                throw new InvalidOperationException("User ID not found in claims or invalid.");
            }

            if(todoList.UsersId.ToString() != userIdClaim.Value.ToString())
            {
                throw new ArgumentException("something wend wrong");
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

            var MyTodoLists = await _todoListsRepository.GetAllMyTodoList(currentUserId);

            var owner = await _usersRepository.GetUserById(currentUserId);

            List<ResponseTodoListsDTO> myDATA = new List<ResponseTodoListsDTO>();

            foreach (var item in MyTodoLists)
            {
                myDATA.Add(new ResponseTodoListsDTO(item.Id, item.Title, owner));
            }

            return myDATA;

        }



        public async Task<ResponseTodoListsDTO> GetTodoListById(int id)
        {
            var todoList = await _todoListsRepository.GetTodoListById(id);
            if (todoList == null)
            {
                throw new ArgumentException("Todo list not found.");
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }

            if (todoList.UsersId.ToString() != userIdClaim.Value.ToString())
            {
                throw new ArgumentException("something wend wrong");

            }

            var ownerDto = await _usersRepository.GetUserById(currentUserId);
            var responseDto = new ResponseTodoListsDTO(todoList.Id, todoList.Title, ownerDto);

            return responseDto;
        }

        public async Task<ResponseTodoListsDTO> UpdateTodoList(UpdateTodoListDTO updateTodoListDTO)
        {
            var todoList = await _todoListsRepository.GetTodoListById(updateTodoListDTO.Id);
            if (todoList == null)
            {
                throw new ArgumentException("Todo list not found.");
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }

            if (todoList.UsersId.ToString() != userIdClaim.Value.ToString())
            {
                throw new ArgumentException("something wend wrong");
            }

            todoList.Title = updateTodoListDTO.Title;

            await _todoListsRepository.UpdateTodoList(todoList);

            var ownerDto = await _usersRepository.GetUserById(currentUserId);

            var responseDto = new ResponseTodoListsDTO(todoList.Id, todoList.Title, ownerDto);

            return responseDto;
        }
    }
}
