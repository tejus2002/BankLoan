using BankLoanProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankLoanProject.Services.Interface
{
    public interface ILoanApplicationService
    {
        List<LoanApplicationViewModel> GetApplications(int customerId);
        List<LoanApplicationViewModel> GetAllApplications();
        void ApplyForLoan(LoanApplicationViewModel model, int customerId);
        List<string> GetProductTypes();
        List<SelectListItem> GetProductNamesByType(string productType);
        void UpdateApplicationStatus(int applicationId, string status);
        int GetApplicationIdByCustomerId(int customerId);




    }
}
