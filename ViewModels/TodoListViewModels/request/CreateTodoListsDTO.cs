using CollaborativeToDoList.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CollaborativeToDoList.ViewModels.TodoListViewModels.request
{
    public record CreateTodoListsDTO
        (
        [NotNull]
        [Required(ErrorMessage = " Title is Required ")]
        string Title,
        string? SharedUrl
        )
    { }
}
