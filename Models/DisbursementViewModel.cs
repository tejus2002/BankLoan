namespace BankLoanProject.Models
{
    public class DisbursementViewModel
    {

        public int ApplicationId { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanTenure { get; set; }
        public decimal LoanAmount { get; set; }
        public string DisburseStatus { get; set; }

    }
}
