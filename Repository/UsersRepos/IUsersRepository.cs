using CollaborativeToDoList.Models;

namespace CollaborativeToDoList.Repository.UsersRepos
{
    public interface IUsersRepository
    {
        Task<Users> CreateUser(Users user);

        Task<Users> UpdateUser(Users user);

        Task<bool> DeleteUser(int id);

        Task<Users> GetUserById(int id);

        Task<IEnumerable<Users>> GetAllUsers();

        Task<Users> GetUserByEmail(string email);
    }
}
