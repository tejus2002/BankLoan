using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankLoanProject.Models.Entities
{
    public class LoanApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("LoanProduct")]
        public int LoanProductId { get; set; }
        public LoanProduct LoanProduct { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal LoanAmount { get; set; }

        public int LoanTenure { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ApplicationDate { get; set; }

        [StringLength(20)]
        [RegularExpression("^(PENDING|APPROVED|REJECTED)$", ErrorMessage = "Approval Status must be PENDING, APPROVED, or REJECTED")]
        public string ApprovalStatus { get; set; }

        public Loan Loan { get; set; } // One-to-one with Loan
        //public ICollection<Repayment> Repayments { get; set; }
    }
}
