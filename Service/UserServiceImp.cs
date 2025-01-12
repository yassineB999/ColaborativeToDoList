using CollaborativeToDoList.Models;
using CollaborativeToDoList.Repository.UsersRepos;
using CollaborativeToDoList.ViewModels.UsersModels.request;
using CollaborativeToDoList.ViewModels.UsersModels.response;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CollaborativeToDoList.Service
{
    public class UserServiceImp : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserServiceImp(IUsersRepository usersRepository, IHttpContextAccessor httpContextAccessor)
        {
            _usersRepository = usersRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DataUserDTO> LoginUser(LoginDTO loginDTO)
        {
            Users user =  _usersRepository.GetUserByEmail(loginDTO.email).Result;

            if (user == null)
            {
                throw new Exception("Invalid email of password");
            }

            if (!BCrypt.Net.BCrypt.EnhancedVerify(loginDTO.password, user.Password))
            {
                throw new Exception("Invalid email of password");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("IsAdmin", user.isAdmin.ToString())
               
            };
            // Create claims identity
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in the user using the claims principal
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Make the cookie persistent
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Set cookie expiration
            };

              await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            DataUserDTO userDTO = new DataUserDTO(user.Id, user.FullName, user.UserName, user.Email);

            return userDTO;

        }

        async Task<DataUserDTO> IUserService.RegisterUser(RegisterDTO registerDTO)
        {

            if (!registerDTO.Password.Equals(registerDTO.ConfirmPassword))
            {
                throw new ArgumentException("Passwords do not match.");
            }

            var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(registerDTO.Password);

            var newUser = new Users
            {
                UserName = registerDTO.Username,
                Password = passwordHash, 
                Email = registerDTO.Email,
                FullName = registerDTO.FullName,
                isAdmin = false
            };

            var createdUser = await _usersRepository.CreateUser(newUser);

            DataUserDTO userDTO = new DataUserDTO(createdUser.Id, createdUser.FullName, createdUser.UserName, createdUser.Email);

            return userDTO;

        }
    }
}
