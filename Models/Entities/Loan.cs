using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankLoanProject.Models.Entities
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }

        [ForeignKey("LoanApplication")]
        public int ApplicationId { get; set; }
        public LoanApplication LoanApplication { get; set; }

        [StringLength(20)]
        [RegularExpression("^(DISBURSE|NOT DISBURSED)$", ErrorMessage = "Disburse Status must be DISBURSE or NOT DISBURSED")]
        public string DisburseStatus { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DisbursementDate { get; set; } // Nullable as it might not be set initially
        [Column(TypeName = "decimal(10, 2)")]
        public decimal OutstandingBalance { get; set; }
        public ICollection<Repayment> Repayments { get; set; }
    }
}
