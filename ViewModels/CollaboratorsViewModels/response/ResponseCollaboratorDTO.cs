namespace CollaborativeToDoList.ViewModels.CollaboratorsViewModels.response
{
    public record ResponseCollaboratorDTO
        (
         int Id,
         int TodoListId,
         int UserId,
         bool CanEdit,
         string UserName,
        bool IsApproved
        )
    {
    }
}
