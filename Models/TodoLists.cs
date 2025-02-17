﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativeToDoList.Models
{
    public class TodoLists
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string? SharedUrl { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users Users { get; set; }

        public ICollection<Collaborators> Collaborators { get; set; } = new List<Collaborators>();
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
}
