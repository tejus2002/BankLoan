using BankLoanProject.Models.Entities;
using BankLoanProject.Models;

namespace BankLoanProject.Services.Interface
{
    public interface IAccountService
    {

        Admin? ValidateAdminLogin(AdminLoginViewModel model);
        Customer? ValidateCustomerLogin(CustomerLoginViewModel model);

    }
}
