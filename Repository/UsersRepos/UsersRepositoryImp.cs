using CollaborativeToDoList.Data;
using CollaborativeToDoList.Models;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeToDoList.Repository.UsersRepos
{
    public class UsersRepositoryImp : IUsersRepository
    {
        private readonly TodoListDbContext _db;

        public UsersRepositoryImp(TodoListDbContext db)
        {
            _db = db;
        }

        async Task<Users> IUsersRepository.GetUserByEmail(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        async Task<Users> IUsersRepository.CreateUser(Users user)
        {
            var existingEmail = _db.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingEmail != null) return null;

            _db.Users.Add(user);

            await _db.SaveChangesAsync();

            return user;
        }

        async Task<bool> IUsersRepository.DeleteUser(int id)
        {
            var dbUser = await _db.Users.FindAsync(id);

            if (dbUser == null) return false;

            _db.Users.Remove(dbUser);
            await _db.SaveChangesAsync();

            return true;
        }

        async Task<IEnumerable<Users>> IUsersRepository.GetAllUsers()
        {
            return await _db.Users.ToListAsync();
        }

        async Task<Users> IUsersRepository.GetUserById(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null) return null;

            return user;
        }

        async Task<Users> IUsersRepository.UpdateUser(Users user)
        {
            var existingUser = await _db.Users.FindAsync(user.Id);

            if (existingUser == null) return null;

            existingUser.FullName = user.FullName;
            existingUser.UserName = user.UserName;

            _db.Users.Update(existingUser);
            await _db.SaveChangesAsync();

            return existingUser;

        }
    }
}
