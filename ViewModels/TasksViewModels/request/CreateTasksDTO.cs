using CollaborativeToDoList.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CollaborativeToDoList.ViewModels.TasksViewModels.request
{
    public record CreateTasksDTO
        (
         int Id,

         [NotNull]
         [Required(ErrorMessage = "Description is required.")]
         string Description,

         DateTime CreatedAt,

         [NotNull]
         [Required(ErrorMessage = "Date is required.")]
         DateTime EndedAt,

         int todoListId,

         [Required(ErrorMessage = "Category is required.")]
         string CategoryName
        )
    {}
}
