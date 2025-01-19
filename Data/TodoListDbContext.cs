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
            modelBuilder.Entity<Collaborators>()
             .HasOne(c => c.Users)
             .WithMany(u => u.Collaborators)
             .HasForeignKey(c => c.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Collaborators>()
                .HasOne(c => c.TodoLists)
                .WithMany(t => t.Collaborators)
                .HasForeignKey(c => c.TodoListId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.TodoLists)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.TodoListId);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Categories)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoriesId);

            modelBuilder.Entity<TodoLists>()
                .HasOne(t => t.Users)
                .WithMany(u => u.TodoLists)
                .HasForeignKey(t => t.UserId);
        }

    }
}
