using CollaborativeToDoList.Service.TodoListsService;
using CollaborativeToDoList.ViewModels.TodoListViewModels.request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollaborativeToDoList.Controllers
{
    [Authorize(Policy = "UserAdmin")] 
    public class TodoListsController : Controller
    {
        private readonly ITodoListsService _todoListsService;

        public TodoListsController(ITodoListsService todoListsService)
        {
            _todoListsService = todoListsService;
        }

        [HttpGet]
        //[Route("")]
        public async Task<IActionResult> TodoHome()
        {
            var todoLists = await _todoListsService.GetAllMyTodoLists();
            return View(todoLists);
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
        [ValidateAntiForgeryToken] // Add anti-forgery token validation
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