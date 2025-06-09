namespace BankLoanProject.Models
{
    public class LoanReportViewModel
    {

        public int ApplicationId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public decimal LoanAmount { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApprovalStatus { get; set; }

    }
}
