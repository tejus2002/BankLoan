namespace BankLoanProject.Models
{
    public class RepaymentScheduleViewModel
    {
        public int RepaymentId { get; set; }
        public int CustomerId { get; set; }
        public int ApplicationId { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public decimal? AmountDue { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal OutstandingBalance { get; set; }
        public string PaymentStatus { get; set; }
        public int LoanId { get; set; }

        public decimal InterestRate { get; set; }

        public decimal LoanAmount { get; set; }
        public int LoanTenure { get; set; }
        
    }
}
