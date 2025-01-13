using CollaborativeToDoList.Models;

namespace CollaborativeToDoList.ViewModels.TodoListViewModels.response
{
    public record ResponseTodoListsDTO
        (
           int Id,
           string Title,
           Users Owner

        )
    {
    }
}
