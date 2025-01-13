using CollaborativeToDoList.ViewModels.UsersModels.request;
using CollaborativeToDoList.ViewModels.UsersModels.response;

namespace CollaborativeToDoList.Service.UserService
{
    public interface IUserService
    {
        Task<DataUserDTO> LoginUser(LoginDTO loginDTO);

        Task<DataUserDTO> RegisterUser(RegisterDTO registerDTO);
    }
}
