using BankLoanProject.Models;

namespace BankLoanProject.Services.Interface
{
    public interface IDisbursementService
    {
        void Disburse(int applicationId);
        List<DisbursementViewModel> GetApprovedApplications(int customerId);
        List<DisbursementViewModel> GetAllApprovedApplications();
    }
}
