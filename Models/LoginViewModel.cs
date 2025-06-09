using System.ComponentModel.DataAnnotations;

namespace BankLoanProject.Models
{
    public class LoginViewModel
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public int AdminId { get; set; }
        [Required]
        public string Phone { get; set; }
        
    }
}
