using BankLoanProject.Data;
using BankLoanProject.Models.Entities;
using BankLoanProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using BankLoanProject.Services.Interface;

namespace BankLoanProject.Services.ModuleService
{
    public class LoanApplicationService : ILoanApplicationService
    {
        private readonly BankLoanDbContext _context;

        public LoanApplicationService(BankLoanDbContext context)
        {
            _context = context;
        }

        public List<LoanApplicationViewModel> GetApplications(int customerId)
        {
            return _context.LoanApplications
                .Where(a => a.CustomerId == customerId)
                .Join(_context.Customers,
                    app => app.CustomerId,
                    cust => cust.CustomerId,
                    (app, cust) => new { app, cust })
                .Join(_context.LoanProducts,
                    ac => ac.app.LoanProductId,
                    prod => prod.LoanProductId,
                    (ac, prod) => new LoanApplicationViewModel
                    {
                        CustomerId = ac.cust.CustomerId,
                        Name = ac.cust.Name,
                        ProductType = prod.ProductType,
                        ProductName = prod.ProductName,
                        InterestRate = prod.InterestRate,
                        LoanAmount = (decimal)ac.app.LoanAmount,
                        LoanTenure = (int)ac.app.LoanTenure,
                        ApplicationDate = ac.app.ApplicationDate,
                        ApprovalStatus = ac.app.ApprovalStatus
                    }).ToList();
        }
        public List<LoanApplicationViewModel> GetAllApplications()
        {
            return _context.LoanApplications
                .Join(_context.Customers,
                    app => app.CustomerId,
                    cust => cust.CustomerId,
                    (app, cust) => new { app, cust })
                .Join(_context.LoanProducts,
                    ac => ac.app.LoanProductId,
                    prod => prod.LoanProductId,
                    (ac, prod) => new LoanApplicationViewModel
                    {
                        CustomerId = ac.cust.CustomerId,
                        Name = ac.cust.Name,
                        ProductType = prod.ProductType,
                        ProductName = prod.ProductName,
                        InterestRate = prod.InterestRate,
                        LoanAmount = (decimal)ac.app.LoanAmount,
                        LoanTenure = (int)ac.app.LoanTenure,
                        ApplicationDate = ac.app.ApplicationDate,
                        ApprovalStatus = ac.app.ApprovalStatus
                    }).ToList();
        }

        public void ApplyForLoan(LoanApplicationViewModel model, int customerId)
        {
            var application = new LoanApplication
            {
                CustomerId = customerId,
                LoanProductId = model.LoanProductId,
                LoanAmount = model.LoanAmount,
                LoanTenure = model.LoanTenure,
                ApplicationDate = DateTime.Today,
                ApprovalStatus = "PENDING"
            };

            _context.LoanApplications.Add(application);
            _context.SaveChanges();

        }

        public List<string> GetProductTypes()
        {
            return _context.LoanProducts
                .Select(p => p.ProductType)
                .Distinct()
                .ToList();
        }

        public List<SelectListItem> GetProductNamesByType(string productType)
        {
            return _context.LoanProducts
                .Where(lp => lp.ProductType == productType)
                .Select(lp => new SelectListItem
                {
                    Text = lp.ProductName,
                    Value = lp.LoanProductId.ToString()
                }).ToList();
        }
        public void UpdateApplicationStatus(int applicationid, string status)
        {
            var application = _context.LoanApplications.FirstOrDefault(a => a.ApplicationId == applicationid);
            if (application != null)
            {
                application.ApprovalStatus = status.ToUpper();
                _context.SaveChanges();

                if (status.Equals("APPROVED", StringComparison.OrdinalIgnoreCase))
                {
                    var loan = new Loan
                    {
                        ApplicationId = applicationid,

                        DisburseStatus = "NOT DISBURSED",
                        DisbursementDate = null,

                        OutstandingBalance = application.LoanAmount
                        // isdisbursed = false and disbursementdate = null are default
                    };
                    _context.Loans.Add(loan);
                    _context.SaveChanges();
                }
            }
        }
        public int GetApplicationIdByCustomerId(int customerId)
        {
            return _context.LoanApplications
                           .Where(a => a.CustomerId == customerId)
                           .Select(a => a.ApplicationId)
                           .FirstOrDefault();
        }

    }




}

