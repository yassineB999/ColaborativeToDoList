using CollaborativeToDoList.Service;
using CollaborativeToDoList.ViewModels.UsersModels.request;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CollaborativeToDoList.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO) 
         {
            if (ModelState.IsValid)
            {
                var user = await _userService.LoginUser(loginDTO);

                if (user == null) return RedirectToAction("Login");

                return RedirectToAction("Register");
            }

            return View(loginDTO);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.RegisterUser(registerDTO);
                return RedirectToAction("Login");
            }

            return View(registerDTO);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

    }
}
