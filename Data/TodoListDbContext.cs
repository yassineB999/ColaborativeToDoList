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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Collaborators: Link to Users with Restrict delete
            modelBuilder.Entity<Collaborators>()
                .HasOne(c => c.Users)
                .WithMany(u => u.Collaborators)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Collaborators: Link to TodoLists with Cascade delete
            modelBuilder.Entity<Collaborators>()
                .HasOne(c => c.TodoLists)
                .WithMany(t => t.Collaborators)
                .HasForeignKey(c => c.TodoListId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tasks: Link to TodoLists
            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.TodoLists)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.TodoListId);

            // Tasks: Link to Categories
            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Categories)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoriesId);

            // TodoLists: Link to Users with Cascade delete
            modelBuilder.Entity<TodoLists>()
                .HasOne(t => t.Users)
                .WithMany(u => u.TodoLists)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
