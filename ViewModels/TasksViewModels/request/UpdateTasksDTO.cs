using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CollaborativeToDoList.ViewModels.TasksViewModels.request
{
    public record UpdateTasksDTO
        (
         int Id,

         [NotNull]
         [Required(ErrorMessage = "Description is required.")]
         string Description,

         [NotNull]
         [Required(ErrorMessage = "Date is required.")]
         DateTime CreatedAt,

         [NotNull]
         [Required(ErrorMessage = "Date is required.")]
         DateTime EndedAt,

         int todoListId,

         string CategoryName
        )
    {}
}
