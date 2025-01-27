using CollaborativeToDoList.Models;

namespace CollaborativeToDoList.Repository.CollaboratorsRepos
{
    public interface ICollaboratorsRepository
    {
        Task<Collaborators> CreateCollaborators(Collaborators collaborators);

        Task<Collaborators> UpdateCollaboratos(Collaborators collaborators);

        Task<Collaborators> DeleteCollaboratos(int id);

        Task<Collaborators> GetCollaboratorById(int id);

        Task<IEnumerable<Collaborators>> GetAllCollaboratos();

        Task<IEnumerable<Collaborators>> GetCollaboratorsByTodoListId(int todoListId);

        Task<Collaborators> GetCollaboratorByTodoListAndUser(int todoListId, int userId);

        Task<IEnumerable<Collaborators>> GetCollaboratorsByUserId(int userId);

        Task<IEnumerable<Collaborators>> GetPendingCollaboratorsByOwner(int ownerId);
    }
}
