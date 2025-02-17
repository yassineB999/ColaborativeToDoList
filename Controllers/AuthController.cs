using CollaborativeToDoList.ViewModels.UsersModels.request;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using CollaborativeToDoList.Service.UserService;

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
        public IActionResult LoginWithRedirect(string returnUrl)
        {
            // Store the return URL (shared URL) in TempData or ViewBag
            ViewBag.ReturnUrl = returnUrl;
            return View("Login"); // Render the login page
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

                return RedirectToAction("TodoHome", "TodoLists");
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
