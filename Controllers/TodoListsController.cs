using CollaborativeToDoList.Repository.CollaboratorsRepos;
using CollaborativeToDoList.Service.CategoriesService;
using CollaborativeToDoList.Service.TasksService;
using CollaborativeToDoList.Service.TodoListsService;
using CollaborativeToDoList.ViewModels.TasksViewModels.request;
using CollaborativeToDoList.ViewModels.TodoListViewModels.request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollaborativeToDoList.Controllers
{
    [Authorize(Policy = "UserAdmin")] 
    public class TodoListsController : Controller
    {
        private readonly ITodoListsService _todoListsService;
        private readonly ITasksService _tasksService;
        private readonly ICategoriesService _categoriesService;
        private readonly ICollaboratorsRepository _collaboratorsRepository;

        public TodoListsController(ITodoListsService todoListsService, ITasksService tasksService, ICategoriesService categoriesService, ICollaboratorsRepository collaboratorsRepository)
        {
            _todoListsService = todoListsService;
            _tasksService = tasksService;
            _categoriesService = categoriesService;
            _collaboratorsRepository = collaboratorsRepository;
        }

        [HttpGet]
        //[Route("")]
        public async Task<IActionResult> TodoHome()
        {
            var myTodoLists = await _todoListsService.GetAllMyTodoLists();

            // Fetch joined to-do lists
            var joinedTodoLists = await _todoListsService.GetJoinedTodoLists();

            // Pass joined to-do lists to the view using ViewBag
            ViewBag.JoinedTodoLists = joinedTodoLists;

            return View(myTodoLists);
        }

        [HttpGet]
        public async Task<IActionResult> TodoListDetails(int Id)
        {
            var tasks = await _tasksService.GetAllTasksInTodoList(new GetAlLTasksDTO(Id));
            var categories = await _categoriesService.GetAllCategories();
            ViewBag.Categories = categories; 
            ViewBag.TodoListId = Id;
            return View(tasks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateTodoListsDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            await _todoListsService.CreateTodoList(dto);
            return Json(new { success = true, message = "Todo list created successfully." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([FromBody] CreateTasksDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            await _tasksService.CreateTaskInTodoList(dto);
            return Json(new { success = true, message = "Task created successfully. " });
        }



        [HttpPut]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Update([FromBody] UpdateTodoListDTO dto)
        {
            if (!ModelState.IsValid)
            {
                
                return Json(new { success = false, message = "Invalid data." });
            }

            await _todoListsService.UpdateTodoList(dto);
            return Json(new { success = true, message = "Todo list updated successfully." });
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTasksDTO dto)
        {
            if (!ModelState.IsValid)
            {

                return Json(new { success = false, message = "Invalid data." });
            }

            await _tasksService.UpdateTaskInTodoList(dto);
            return Json(new { success = true, message = "Task updated successfully." });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromBody] DeletetodoListsDTO dto)
        {
            await _todoListsService.DeleteTodoList(dto);
            return Json(new { success = true, message = "Todo list deleted successfully." });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTask([FromBody] DeleteTasksDTO dto)
        {
            await _tasksService.DeleteTaskInTodoList(dto);
            return Json(new { success = true, message = "Task deleted successfully." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinTodoList([FromBody] JoinTodoListDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            try
            {
                await _todoListsService.JoinTodoListBySharedUrl(dto);
                return Json(new { success = true, message = "Successfully joined the to-do list." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> PendingCollaborators()
        {
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("User ID not found in claims or invalid.");
            }

            try
            {
                var pendingCollaborators = await _todoListsService.GetPendingCollaboratorsByOwnerId(userId);
                return View(pendingCollaborators);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveCollaborator(int collaboratorId)
        {
            try
            {
                await _todoListsService.ApproveCollaborator(collaboratorId);
                return Json(new { success = true, message = "Collaborator approved." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectCollaborator(int collaboratorId)
        {
            try
            {
                await _todoListsService.RejectCollaborator(collaboratorId);
                return Json(new { success = true, message = "Collaborator rejected." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveTodoList([FromBody] int todoListId)
        {
            try
            {
                await _todoListsService.LeaveTodoList(todoListId);
                return Json(new { success = true, message = "Successfully left the to-do list." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
