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

        public bool IsApproved { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users Users { get; set; }

        [ForeignKey("TodoLists")]
        public int TodoListId { get; set; }
        public TodoLists TodoLists { get; set; }

    }
}
