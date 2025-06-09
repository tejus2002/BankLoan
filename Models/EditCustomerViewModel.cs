using System.ComponentModel.DataAnnotations;

namespace BankLoanProject.Models
{
    public class EditCustomerViewModel
    {
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string IdentityType { get; set; }

        [Required]
        public string KycStatus { get; set; }
    }
}
