using System.ComponentModel.DataAnnotations;

namespace BankLoanProject.Models
{
    public class AdminLoginViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public int AdminId { get; set; }
    }
}
