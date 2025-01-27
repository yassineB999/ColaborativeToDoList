using CollaborativeToDoList.Data;
using CollaborativeToDoList.Models;
using CollaborativeToDoList.Repository.CollaboratorsRepos;
using CollaborativeToDoList.Repository.TodoListsRepos;
using CollaborativeToDoList.Repository.UsersRepos;
using CollaborativeToDoList.ViewModels.CollaboratorsViewModels.response;
using CollaborativeToDoList.ViewModels.TodoListViewModels.request;
using CollaborativeToDoList.ViewModels.TodoListViewModels.response;

namespace CollaborativeToDoList.Service.TodoListsService
{
    public class TodoListsServiceImp : ITodoListsService
    {
        private readonly ITodoListsRepository _todoListsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICollaboratorsRepository _collaboratorsRepository;
        private readonly Utils _utils;

        public TodoListsServiceImp(ITodoListsRepository todoListsRepository, IUsersRepository usersRepository, IHttpContextAccessor httpContextAccessor, ICollaboratorsRepository collaboratorsRepository, Utils utils)
        {
            _todoListsRepository = todoListsRepository;
            _usersRepository = usersRepository;
            _httpContextAccessor = httpContextAccessor;
            _collaboratorsRepository = collaboratorsRepository;
            _utils = utils;
        }

        public async Task<ResponseTodoListsDTO> CreateTodoList(CreateTodoListsDTO createTodoListsDTO)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int UsersId))
            {
                throw new InvalidOperationException("User ID not found in claims or invalid.");
            }

            var baseUrl = "http://localhost:5262";
            var sharedUrl = _utils.GenerateSharedUrl(baseUrl, createTodoListsDTO.Title);

            var todoList = new TodoLists
            {
                Title = createTodoListsDTO.Title,
                UserId = UsersId,
                SharedUrl = sharedUrl
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
                OwnerName: owner.FullName,
                SharedUrl: sharedUrl
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
                    OwnerName: owner.FullName,
                    SharedUrl: item.SharedUrl
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
                OwnerName: owner.FullName,
                SharedUrl: todoList.SharedUrl
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
                OwnerName: owner.FullName,
                SharedUrl: todoList.SharedUrl
            );

            return responseDto;
        }

         public async Task<IEnumerable<ResponseTodoListsDTO>> GetJoinedTodoLists()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
        {
            throw new InvalidOperationException("User ID not found or invalid.");
        }

        var joinedTodoLists = await _todoListsRepository.GetJoinedTodoLists(currentUserId);

        var response = joinedTodoLists
            .Select(t => new ResponseTodoListsDTO(
                Id: t.Id,
                Title: t.Title,
                OwnerId: t.UserId,
                OwnerName: t.Users.FullName,
                SharedUrl: t.SharedUrl
            ))
            .ToList();

        return response;
    }

        public async Task<IEnumerable<ResponseCollaboratorDTO>> GetPendingCollaboratorsByOwnerId(int ownerId)
        {
            var pendingCollaborators = await _collaboratorsRepository.GetPendingCollaboratorsByOwner(ownerId);

            return pendingCollaborators.Select(c => new ResponseCollaboratorDTO(
                Id: c.Id,
                TodoListId: c.TodoListId,
                UserId: c.UserId,
                CanEdit: c.CanEdit,
                UserName: c.Users.UserName,  // Assuming Include is used in repository
                IsApproved: c.IsApproved
            )).ToList();
        }


        public async Task JoinTodoListBySharedUrl(JoinTodoListDTO joinTodoListDTO)
        {
            if (joinTodoListDTO == null || string.IsNullOrWhiteSpace(joinTodoListDTO.SharedUrl))
            {
                throw new ArgumentException("Shared URL is required.");
            }

            // Get the current user ID from the claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }

            // Get the to-do list by shared URL
            var todoList = await _todoListsRepository.GetTodoListBySharedUrl(joinTodoListDTO.SharedUrl);
            if (todoList == null)
            {
                throw new KeyNotFoundException("To-do list not found for the given shared URL.");
            }

            // Check if the current user is the owner of the to-do list
            if (todoList.UserId == currentUserId)
            {
                throw new InvalidOperationException("You are the owner of this to-do list and cannot be a collaborator.");
            }

            // Check if the user is already a collaborator
            var existingCollaborator = await _collaboratorsRepository.GetCollaboratorByTodoListAndUser(todoList.Id, currentUserId);
            if (existingCollaborator != null)
            {
                throw new InvalidOperationException("You are already a collaborator on this to-do list.");
            }

            // Create a new collaborator
            var collaborator = new Collaborators
            {
                UserId = currentUserId,
                TodoListId = todoList.Id,
                CanEdit = false, // Default to no edit permissions
                IsApproved = false // Default to pending approval
            };

            await _collaboratorsRepository.CreateCollaborators(collaborator);
        }

        public async Task ApproveCollaborator(int collaboratorId)
        {
            // Get the collaborator
            var collaborator = await _collaboratorsRepository.GetCollaboratorById(collaboratorId);
            if (collaborator == null)
            {
                throw new KeyNotFoundException("Collaborator not found.");
            }

            // Get the TodoList to check ownership
            var todoList = await _todoListsRepository.GetTodoListById(collaborator.TodoListId);

            // Get current user ID from claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }

            // Ensure the current user is the owner
            if (todoList.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("You are not the owner of this TodoList.");
            }

            // Approve the collaborator
            collaborator.IsApproved = true;
            await _collaboratorsRepository.UpdateCollaboratos(collaborator);
        }

        public async Task RejectCollaborator(int collaboratorId)
        {
            // Get the collaborator
            var collaborator = await _collaboratorsRepository.GetCollaboratorById(collaboratorId);
            if (collaborator == null)
            {
                throw new KeyNotFoundException("Collaborator not found.");
            }

            // Get the TodoList to check ownership
            var todoList = await _todoListsRepository.GetTodoListById(collaborator.TodoListId);

            // Get current user ID from claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }

            // Ensure the current user is the owner
            if (todoList.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("You are not the owner of this TodoList.");
            }

            // Reject (delete) the collaborator
            await _collaboratorsRepository.DeleteCollaboratos(collaborator.Id);
        }

        public async Task LeaveTodoList(int todoListId)
        {
            // Get the current user ID from the claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                throw new InvalidOperationException("User ID not found or invalid.");
            }

            // Get the to-do list by ID
            var todoList = await _todoListsRepository.GetTodoListById(todoListId);
            if (todoList == null)
            {
                throw new KeyNotFoundException("To-do list not found.");
            }

            // Check if the current user is the owner of the to-do list
            if (todoList.UserId == currentUserId)
            {
                throw new InvalidOperationException("You are the owner of this to-do list and cannot leave it.");
            }

            // Check if the user is a collaborator
            var existingCollaborator = await _collaboratorsRepository.GetCollaboratorByTodoListAndUser(todoListId, currentUserId);
            if (existingCollaborator == null)
            {
                throw new InvalidOperationException("You are not a collaborator on this to-do list.");
            }

            // Remove the collaborator
            await _collaboratorsRepository.DeleteCollaboratos(existingCollaborator.Id);
        }
    }
}

