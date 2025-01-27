using CollaborativeToDoList.Data;
using CollaborativeToDoList.Models;
using CollaborativeToDoList.Repository.CategoriesRepos;
using CollaborativeToDoList.Repository.CollaboratorsRepos;
using CollaborativeToDoList.Repository.TasksRepos;
using CollaborativeToDoList.Repository.TodoListsRepos;
using CollaborativeToDoList.Repository.UsersRepos;
using CollaborativeToDoList.Service.CategoriesService;
using CollaborativeToDoList.Service.TasksService;
using CollaborativeToDoList.Service.TodoListsService;
using CollaborativeToDoList.Service.UserService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeToDoList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("db_one");

            builder.Services.AddDbContext<TodoListDbContext>(options => options.UseSqlServer(
                connectionString
                ));

            // Add authentication with cookie options
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true; // Prevent client-side script access
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Require HTTPS
                    options.Cookie.SameSite = SameSiteMode.Strict; // Prevent CSRF
                    options.LoginPath = "/Auth/Login"; // Redirect to login page if unauthorized
                    options.AccessDeniedPath = "/Auth/AccessDenied"; // Redirect to access denied page
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin", "True"));
                options.AddPolicy("UserAdmin", policy => policy.RequireClaim("IsAdmin", "False"));
            });

            /*
            builder.Services.AddAntiforgery(options =>
            {
                options.FormFieldName = "__RequestVerificationToken"; // Par défaut
                options.HeaderName = "X-CSRF-TOKEN"; // Pour les requêtes AJAX
                options.Cookie.Name = "MyAntiforgeryCookie";
            });
*/

            builder.Services.AddScoped<IUsersRepository, UsersRepositoryImp>();
            builder.Services.AddScoped<IUserService, UserServiceImp>();
            builder.Services.AddScoped<ITodoListsRepository, TodoListsRepositoryImp>();
            builder.Services.AddScoped<ITodoListsService, TodoListsServiceImp>();
            builder.Services.AddScoped<ITasksRepository, TasksRepositoryImp>();
            builder.Services.AddScoped<ITasksService, TasksServiceImp>();
            builder.Services.AddScoped<ICategoriesRepository, CategoriesRepositoryImp>();
            builder.Services.AddScoped<ICategoriesService, CategoriesServiceImp>();
            builder.Services.AddScoped<ICollaboratorsRepository, CollaboratorsRepositoryImp>();
            builder.Services.AddScoped<Utils>();
            // Register IHttpContextAccessor to access HttpContext in services
            builder.Services.AddHttpContextAccessor();

            ConfigureServices(builder.Services);

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Add session middleware
            app.UseSession();

            Configure(app);

            app.Run();
        }


        /// <summary>
        /// Configures services for the ASP.NET Core application, including controllers with views and session services.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

        }


        /// <summary>
        /// Configures middleware components for the ASP.NET Core application, including error handling, static files, session, routing, and controllers.
        /// </summary>
        /// <param name="app">The application builder used to configure the application's request pipeline.</param>
        private static void Configure(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/E404");
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Error/E404";
                    await next();
                }
            });

            app.UseStaticFiles();

            app.UseRouting();

            // Add authentication middleware (must be before UseAuthorization)
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
            name: "sharedUrl",
            pattern: "Home/AccessSharedUrl",
            defaults: new { controller = "Home", action = "AccessSharedUrl" });
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");
        }
    }
}
