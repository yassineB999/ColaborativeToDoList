using CollaborativeToDoList.Service.TodoListsService;
using CollaborativeToDoList.ViewModels.TodoListViewModels.request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollaborativeToDoList.Controllers
{
    [Controller]
    public class TodoListsController : Controller
    {

        private readonly ITodoListsService _todoListsService;

        public TodoListsController(ITodoListsService todoListsService)
        {
            _todoListsService = todoListsService;
        }

        [HttpGet("TodoHome")]
        [Authorize(Policy = "UserAdmin")]
        public async Task<IActionResult> TodoHome()
        {
            var todoLists = await _todoListsService.GetAllMyTodoLists();
            return View(todoLists);
        }

        [HttpGet("GetAllMyTodoLists")]
        [Authorize(Policy = "UserAdmin")]
        public async Task<IActionResult> GetAllMyTodoLists()
        {
            var todoLists = await _todoListsService.GetAllMyTodoLists();
            return Ok(todoLists);
        }

        [HttpPost("Create")]
        [Authorize(Policy = "UserAdmin")]
        public async Task<IActionResult> Create(CreateTodoListsDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("TodoHome");
            }

            await _todoListsService.CreateTodoList(dto);
            return RedirectToAction(nameof(TodoHome));
        }

        [HttpGet("GetTodoListById/{id}")]
        [Authorize(Policy = "UserAdmin")]
        public async Task<IActionResult> GetTodoListById(int id)
        {
            var todoList = await _todoListsService.GetTodoListById(id);
            if (todoList == null)
            {
                return NotFound();
            }
            return Ok(todoList);
        }

        [HttpPut("Update")]
        [Authorize(Policy = "UserAdmin")]
        public async Task<IActionResult> Update(UpdateTodoListDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("TodoHome");
            }

            await _todoListsService.UpdateTodoList(dto);
            return RedirectToAction(nameof(TodoHome));
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Policy = "UserAdmin")]
        public async Task<IActionResult> DeleteTodoList(DeletetodoListsDTO deletetodoListsDTO)
        {
            var deletetodoLists = _todoListsService.DeleteTodoList(deletetodoListsDTO);
            await _todoListsService.DeleteTodoList(deletetodoListsDTO);
            return NoContent();
        }
    }
}
