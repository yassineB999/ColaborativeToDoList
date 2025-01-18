using CollaborativeToDoList.Models;

namespace CollaborativeToDoList.ViewModels.TodoListViewModels.response
{
    public record ResponseTodoListsDTO
        (
           int Id,
           string Title,
           int OwnerId,
           string OwnerName

        )
    {
    }
}
