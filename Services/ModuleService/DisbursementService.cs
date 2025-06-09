using BankLoanProject.Data;
using BankLoanProject.Models;
using BankLoanProject.Services.Interface;

namespace BankLoanProject.Services.ModuleService
{
    public class DisbursementService: IDisbursementService
    {

        private readonly BankLoanDbContext _context;

        public DisbursementService(BankLoanDbContext context)
        {
            _context = context;
        }

        public List<DisbursementViewModel> GetApprovedApplications(int customerId)
        {
            return _context.LoanApplications
                .Where(app => app.ApprovalStatus == "APPROVED" && app.CustomerId == customerId)
                .Join(_context.Customers,
                    app => app.CustomerId,
                    cust => cust.CustomerId,
                    (app, cust) => new { app, cust })
                .Join(_context.LoanProducts,
                    ac => ac.app.LoanProductId,
                    prod => prod.LoanProductId,
                    (ac, prod) => new { ac.app, ac.cust, prod })
                .Join(_context.Loans,
                    acp => acp.app.ApplicationId,
                    loan => loan.ApplicationId,
                    (acp, loan) => new DisbursementViewModel
                    {
                        ApplicationId = acp.app.ApplicationId,
                        CustomerId = acp.cust.CustomerId,
                        Name = acp.cust.Name,
                        ProductType = acp.prod.ProductType,
                        ProductName = acp.prod.ProductName,
                        InterestRate = acp.prod.InterestRate,
                        LoanTenure = acp.app.LoanTenure,
                        LoanAmount = acp.app.LoanAmount,
                        DisburseStatus = loan.DisburseStatus
                    })
                .ToList();
        }


        public List<DisbursementViewModel> GetAllApprovedApplications()
        {
            return _context.LoanApplications
                .Where(app => app.ApprovalStatus == "APPROVED")
                .Join(_context.Customers,
                    app => app.CustomerId,
                    cust => cust.CustomerId,
                    (app, cust) => new { app, cust })
                .Join(_context.LoanProducts,
                    ac => ac.app.LoanProductId,
                    prod => prod.LoanProductId,
                    (ac, prod) => new { ac.app, ac.cust, prod })
                .Join(_context.Loans,
                    acp => acp.app.ApplicationId,
                    loan => loan.ApplicationId,
                    (acp, loan) => new DisbursementViewModel
                    {
                        ApplicationId = acp.app.ApplicationId,
                        CustomerId = acp.cust.CustomerId,
                        Name = acp.cust.Name,
                        ProductType = acp.prod.ProductType,
                        ProductName = acp.prod.ProductName,
                        InterestRate = acp.prod.InterestRate,
                        LoanTenure = acp.app.LoanTenure,
                        LoanAmount = acp.app.LoanAmount,
                        DisburseStatus = loan.DisburseStatus
                    })
                .ToList();
        }




        public void Disburse(int applicationId)
        {
            var loan = _context.Loans.FirstOrDefault(l => l.ApplicationId == applicationId);
            if (loan != null)
            {
                loan.DisbursementDate = DateTime.Today;
                loan.DisburseStatus = "DISBURSED";
                _context.SaveChanges();
            }
        }


    }
}
