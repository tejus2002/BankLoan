using BankLoanProject.Data;
using BankLoanProject.Models;
using BankLoanProject.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace BankLoanProject.Services.ModuleService
{
    public class ReportService:IReportService
    {
        private readonly BankLoanDbContext _context;

        public ReportService(BankLoanDbContext context)
        {
            _context = context;
        }

        public List<LoanReportViewModel> GenerateLoanReport()
        {
            return _context.LoanApplications
            .Include(a => a.Customer)
            .Include(a => a.LoanProduct)
            .Select(a => new LoanReportViewModel
            {
                ApplicationId = a.ApplicationId,
                CustomerId = a.CustomerId,
                CustomerName = a.Customer.Name,
                ProductType = a.LoanProduct.ProductType,
                ProductName = a.LoanProduct.ProductName,
                LoanAmount = a.LoanAmount,
                ApplicationDate = a.ApplicationDate,
                ApprovalStatus = a.ApprovalStatus
            }).ToList();
        }

        public List<OutstandingReportViewModel> GenerateOutstandingReport()
        {
            return _context.Loans
            .Include(l => l.LoanApplication)
            .ThenInclude(a => a.Customer)
            .Include(l => l.LoanApplication.LoanProduct)
            .Select(l => new OutstandingReportViewModel
            {
                CustomerId = l.LoanApplication.Customer.CustomerId,
                CustomerName = l.LoanApplication.Customer.Name,
                LoanId = l.LoanId,
                ProductName = l.LoanApplication.LoanProduct.ProductName,
                ProductType = l.LoanApplication.LoanProduct.ProductType,
                LoanAmount = l.LoanApplication.LoanAmount,
                OutstandingBalance = l.OutstandingBalance
            }).ToList();
        }

        public List<RepaymentReportViewModel> GenerateRepaymentReport()
        {
            return _context.Repayments
            .Include(r => r.Loan)
            .ThenInclude(l => l.LoanApplication)
            .ThenInclude(a => a.Customer)
            .Select(r => new RepaymentReportViewModel
            {
                RepaymentId = r.RepaymentId,
                CustomerId = r.Loan.LoanApplication.Customer.CustomerId,
                CustomerName = r.Loan.LoanApplication.Customer.Name,
                LoanId = r.LoanId,
                AmountDue = r.AmountDue,
                DueDate = r.DueDate,
                PaymentDate = r.PaymentDate,
                PaymentStatus = r.PaymentStatus
            }).ToList();
        }

        public List<LoanReportViewModel> GenerateLoanReportByCustomerId(int customerId)
        {
            return _context.LoanApplications
                .Include(a => a.Customer)
                .Include(a => a.LoanProduct)
                .Where(a => a.CustomerId == customerId)
                .Select(a => new LoanReportViewModel
                {
                    ApplicationId = a.ApplicationId,
                    CustomerId = a.CustomerId,
                    CustomerName = a.Customer.Name,
                    ProductType = a.LoanProduct.ProductType,
                    ProductName = a.LoanProduct.ProductName,
                    LoanAmount = a.LoanAmount,
                    ApplicationDate = a.ApplicationDate,
                    ApprovalStatus = a.ApprovalStatus
                }).ToList();
        }

        public List<OutstandingReportViewModel> GenerateOutstandingReportByCustomerId(int customerId)
        {
            return _context.Loans
                .Include(l => l.LoanApplication)
                .ThenInclude(a => a.Customer)
                .Include(l => l.LoanApplication.LoanProduct)
                .Where(l => l.LoanApplication.Customer.CustomerId == customerId)
                .Select(l => new OutstandingReportViewModel
                {
                    CustomerId = l.LoanApplication.Customer.CustomerId,
                    CustomerName = l.LoanApplication.Customer.Name,
                    LoanId = l.LoanId,
                    ProductName = l.LoanApplication.LoanProduct.ProductName,
                    ProductType = l.LoanApplication.LoanProduct.ProductType,
                    LoanAmount = l.LoanApplication.LoanAmount,
                    OutstandingBalance = l.OutstandingBalance
                }).ToList();
        }

        public List<RepaymentReportViewModel> GenerateRepaymentReportByCustomerId(int customerId)
        {
            return _context.Repayments
                .Include(r => r.Loan)
                .ThenInclude(l => l.LoanApplication)
                .ThenInclude(a => a.Customer)
                .Where(r => r.Loan.LoanApplication.Customer.CustomerId == customerId)
                .Select(r => new RepaymentReportViewModel
                {
                    RepaymentId = r.RepaymentId,
                    CustomerId = r.Loan.LoanApplication.Customer.CustomerId,
                    CustomerName = r.Loan.LoanApplication.Customer.Name,
                    LoanId = r.LoanId,
                    AmountDue = r.AmountDue,
                    DueDate = r.DueDate,
                    PaymentDate = r.PaymentDate,
                    PaymentStatus = r.PaymentStatus
                }).ToList();
        }



    }
}
