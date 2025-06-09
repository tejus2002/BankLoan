using System.ComponentModel.DataAnnotations;

namespace BankLoanProject.Models.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(15)]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression("^(PENDING|VERIFIED)$", ErrorMessage = "KYC Status must be PENDING or VERIFIED")]
        public string KycStatus { get; set; }

        [Required]
        // Changed: Max length 15, Min length 8 (you can adjust min length as needed)
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 15 characters long.")]
        public string Password { get; set; }
        [Required]
        public string IdentityType { get; set; }

        public string DocumentPath { get; set; }
        public string Role { get; set; } = "Customer";
        public ICollection<LoanApplication> LoanApplications { get; set; }

    }
}
