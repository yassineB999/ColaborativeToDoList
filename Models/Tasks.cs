using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativeToDoList.Models
{
    public class Tasks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime EndedAt { get; set; }

        [ForeignKey("TodoLists")]
        public int TodoListId { get; set; }
        public TodoLists TodoLists { get; set; }

        [ForeignKey("Categories")]
        public int CategoriesId { get; set; }
        public Categories Categories { get; set; }
    }
}
