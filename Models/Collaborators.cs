using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativeToDoList.Models
{
    public class Collaborators
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }

        public bool CanEdit { get; set; }

        [ForeignKey("Users")]

        public int userId;
        public Users users;

        [ForeignKey("TodoLists")]

        public int todoListId;
        public TodoLists todoLists;

    }
}
