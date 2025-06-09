using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankLoanProject.Models.Entities
{
    public class Repayment

    {
        [Key]
        public int RepaymentId { get; set; }

        [ForeignKey("Loan")]
        public int LoanId { get; set; }
        public Loan Loan { get; set; }

        public int ApplicationId { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        public LoanApplication LoanApplication { get; set; }


        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal AmountDue { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaymentDate { get; set; }

        [StringLength(20)]
        [RegularExpression("^(PENDING|COMPLETED)$")]
        public string PaymentStatus { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? PaidAmount { get; set; }


    }
}
