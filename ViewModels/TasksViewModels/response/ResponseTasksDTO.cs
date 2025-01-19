using CollaborativeToDoList.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CollaborativeToDoList.ViewModels.TasksViewModels.response
{
    public record ResponseTasksDTO
    (
      int Id,
     string Description,
     DateTime CreatedAt,
     DateTime? EndedAt,
     string CategoryName
        )
    { }
}
