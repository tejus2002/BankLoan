using System.ComponentModel.DataAnnotations;

namespace BankLoanProject.Models
{
    public class CustomerLoginViewModel
    {
        
        [Required]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
