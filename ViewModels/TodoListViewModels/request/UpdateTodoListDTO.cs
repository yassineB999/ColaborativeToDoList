using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CollaborativeToDoList.ViewModels.TodoListViewModels.request
{
    public record UpdateTodoListDTO
        (

        int Id,

        [NotNull]
        [Required(ErrorMessage = " Title is Required ")]
        string Title
        )
    { }
}
