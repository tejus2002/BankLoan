namespace BankLoanProject.Models
{
    public class OutstandingReportViewModel
    {

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int LoanId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal OutstandingBalance { get; set; }

    }
}
