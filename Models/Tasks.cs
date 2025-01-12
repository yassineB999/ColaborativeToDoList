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
        public int todoListId;
        public TodoLists todoLists;

        [ForeignKey("Categories")]

        public int CategoriesId;
        public Categories categories;
    }
}
