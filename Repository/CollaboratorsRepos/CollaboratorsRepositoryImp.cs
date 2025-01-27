using CollaborativeToDoList.Data;
using CollaborativeToDoList.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CollaborativeToDoList.Repository.CollaboratorsRepos
{
    public class CollaboratorsRepositoryImp : ICollaboratorsRepository
    {
        private readonly TodoListDbContext _db;
        public CollaboratorsRepositoryImp(TodoListDbContext db) 
        {
            _db = db;
        }

        public async Task<Collaborators> CreateCollaborators(Collaborators collaborators)
        {
            if(collaborators == null)
            {
                throw new ArgumentNullException(nameof(collaborators));
            }
            _db.Collaborators.Add(collaborators);
            await _db.SaveChangesAsync();
            return collaborators;
        }

        public async Task<Collaborators> DeleteCollaboratos(int id)
        {
            var collaborator = await _db.Collaborators.FindAsync(id);
            if (collaborator == null)
            {
                throw new KeyNotFoundException("Collaborator not found.");
            }

            _db.Collaborators.Remove(collaborator);
            await _db.SaveChangesAsync();
            return collaborator;

        }

        public async Task<IEnumerable<Collaborators>> GetAllCollaboratos()
        {
            return await _db.Collaborators.ToListAsync();
        }

        public async Task<Collaborators> GetCollaboratorById(int id)
        {
            var collaborator = await _db.Collaborators.FindAsync(id);
            if (collaborator == null)
            {
                throw new KeyNotFoundException("Collaborator not found.");
            }
            return collaborator;
        }

        public async Task<IEnumerable<Collaborators>> GetCollaboratorsByTodoListId(int todoListId)
        {

           var collaborators = await _db.Collaborators.Where(c => c.TodoListId == todoListId).ToListAsync();

            if(collaborators == null)
            {
                throw new KeyNotFoundException("Collaborator not found.");
            }
            return collaborators;
        }

        public async Task<Collaborators> UpdateCollaboratos(Collaborators collaborators)
        {
            if (collaborators == null)
            {
                throw new ArgumentNullException(nameof(collaborators));
            }

            var existingCollaborator = await _db.Collaborators.FindAsync(collaborators.Id);
            if (existingCollaborator == null)
            {
                throw new KeyNotFoundException("Collaborator not found.");
            }

            existingCollaborator.CanEdit = collaborators.CanEdit;
            existingCollaborator.IsApproved = collaborators.IsApproved;

            _db.Collaborators.Update(existingCollaborator);
            await _db.SaveChangesAsync();

            return existingCollaborator;
        }

        public async Task<Collaborators> GetCollaboratorByTodoListAndUser(int todoListId, int userId)
        {
            var collaborator = await _db.Collaborators
                .FirstOrDefaultAsync(c => c.TodoListId == todoListId && c.UserId == userId);

            // Return null if no collaborator is found
            return collaborator;
        }

        public async Task<IEnumerable<Collaborators>> GetCollaboratorsByUserId(int userId)
        {
            return await _db.Collaborators
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Collaborators>> GetPendingCollaboratorsByOwner(int ownerId)
        {
            return await _db.Collaborators
                .Include(c => c.TodoLists)
                .Include(c => c.Users)  // Include user data
                .Where(c =>
                    c.TodoLists.UserId == ownerId &&
                    !c.IsApproved &&
                    c.TodoLists != null
                )
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
