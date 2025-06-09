using BankLoanProject.Data;
using BankLoanProject.Models.Entities;
using BankLoanProject.Models;
using BankLoanProject.Services.Interface;

namespace BankLoanProject.Services.ModuleService
{
    public class AccountService:IAccountService
    {

        private readonly BankLoanDbContext _db;

        public AccountService(BankLoanDbContext db)
        {
            _db = db;
        }

        public Admin? ValidateAdminLogin(AdminLoginViewModel model)
        {
            return _db.Admins.FirstOrDefault(a => a.AdminId == model.AdminId && a.Password == model.Password);
        }

        public Customer? ValidateCustomerLogin(CustomerLoginViewModel model)
        {
            return _db.Customers.FirstOrDefault(c => c.Phone == model.Phone && c.Password == model.Password);
        }

    }
}
