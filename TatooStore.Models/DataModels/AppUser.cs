using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TatooStore.Models.DataModels
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(64)]
        public string FirstName { get; set; }

        [MaxLength(64)]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(64)]
        public string LastName { get; set; }

        [Required]
        public bool IsActiveUser { get; set; }

        public Customer? Customer { get; set; }
        public Employee? Employee { get; set; }
    }
}
