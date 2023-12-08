using System.ComponentModel.DataAnnotations;

namespace TatooStore.Models.ViewModels.Account.Registration
{
    public class CustomerRegistration
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
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(16)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
