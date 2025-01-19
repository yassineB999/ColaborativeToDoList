using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CollaborativeToDoList.ViewModels.UsersModels.request
{
    public record LoginDTO(

        [NotNull]
        [Required(ErrorMessage = "Email address is required")]
        [Display(Name = "Email Address")]
        string email,

        [NotNull]
        [Required]
        [DataType(DataType.Password)]
        string password
        ) { }
}
