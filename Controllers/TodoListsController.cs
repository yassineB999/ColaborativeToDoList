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

        public TodoListsController(ITodoListsService todoListsService, ITasksService tasksService)
        {
            _todoListsService = todoListsService;
            _tasksService = tasksService;
        }

        [HttpGet]
        //[Route("")]
        public async Task<IActionResult> TodoHome()
        {
            var todoLists = await _todoListsService.GetAllMyTodoLists();
            return View(todoLists);
        }

        [HttpGet]
        public async Task<IActionResult> TodoListDetails(int Id)
        {
            var tasks = await _tasksService.GetAllTasksInTodoList(new GetAlLTasksDTO(Id));
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

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromBody] DeletetodoListsDTO dto)
        {
            await _todoListsService.DeleteTodoList(dto);
            return Json(new { success = true, message = "Todo list deleted successfully." });
        }
    }
}