namespace BankLoanProject.Models
{
    public class RepaymentReportViewModel
    {

        public int RepaymentId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int LoanId { get; set; }
        public decimal AmountDue { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentStatus { get; set; }

    }
}
