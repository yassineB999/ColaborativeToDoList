using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativeToDoList.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public bool isAdmin { get; set; } = false;

        public ICollection<TodoLists> TodoLists { get; set; } = new List<TodoLists>();
        public ICollection<Collaborators> Collaborators { get; set; } = new List<Collaborators>();
    }

}
