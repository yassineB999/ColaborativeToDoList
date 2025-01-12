using CollaborativeToDoList.Models;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeToDoList.Data
{
    public class TodoListDbContext : DbContext
    {
        public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }

        public DbSet<TodoLists> TodoLists { get; set; } 

        public DbSet<Categories> Categories { get; set; }

        public DbSet<Tasks> Tasks { get; set; }

        public DbSet<Collaborators> Collaborators { get; set; }
    }
}
