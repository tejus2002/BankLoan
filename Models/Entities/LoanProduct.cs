using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankLoanProject.Models.Entities
{
    public class LoanProduct
    {
        [Key]
        public int LoanProductId { get; set; }

        [StringLength(50)]
        public string ProductType { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal InterestRate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal MinAmount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal MaxAmount { get; set; }

        public int Tenure { get; set; }

        //public ICollection<LoanApplication> LoanApplications { get; set; }
    }
}
