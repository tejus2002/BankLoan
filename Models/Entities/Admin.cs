using System.ComponentModel.DataAnnotations;

namespace BankLoanProject.Models.Entities
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        // Changed: Max length 15, Min length 8 (you can adjust min length as needed)
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 15 characters long.")]
        public string Password { get; set; }
        public string Role { get; set; } = "Admin";

    }
}
