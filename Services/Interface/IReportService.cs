using BankLoanProject.Models;

namespace BankLoanProject.Services.Interface
{
    public interface IReportService
    {

        List<LoanReportViewModel> GenerateLoanReport();
        List<OutstandingReportViewModel> GenerateOutstandingReport();
        List<RepaymentReportViewModel> GenerateRepaymentReport();
        List<LoanReportViewModel> GenerateLoanReportByCustomerId(int customerId);
        List<OutstandingReportViewModel> GenerateOutstandingReportByCustomerId(int customerId);
        List<RepaymentReportViewModel> GenerateRepaymentReportByCustomerId(int customerId);


    }
}
