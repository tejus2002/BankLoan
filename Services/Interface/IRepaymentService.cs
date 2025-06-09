using BankLoanProject.Models;

namespace BankLoanProject.Services.Interface
{
    public interface IRepaymentService
    {
        Task<List<RepaymentScheduleViewModel>> GetRepaymentScheduleAsync(int applicationId);

        Task GenerateScheduleAsync(int applicationId);
        Task<List<RepaymentScheduleViewModel>> GetLoansForSchedulingAsync();
        Task<RepaymentScheduleViewModel> MakePaymentAsync(int repaymentId);
        Task<decimal?> GetOutstandingBalanceAsync(int loanId);
    }
}
