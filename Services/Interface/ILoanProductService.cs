using BankLoanProject.Models.Entities;

namespace BankLoanProject.Services.Interface
{
    public interface ILoanProductService
    {
        IEnumerable<LoanProduct> GetAll();
        LoanProduct GetById(int id);
        void Add(LoanProduct product);
        void Update(LoanProduct product);
        void Delete(int id);
    }
}
