using System.ComponentModel.DataAnnotations;

namespace TatooStore.Models.DataModels
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string UserId { get; set; }
        public AppUser? User { get; set; }

        public IList<Purchase>? Purchases { get; set; }
    }
}
