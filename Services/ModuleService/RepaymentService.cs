
using Microsoft.EntityFrameworkCore;

using BankLoanProject.Data;
using BankLoanProject.Models.Entities;
using BankLoanProject.Models;
using BankLoanProject.Services.Interface;

namespace BankLoanProject.Services
{
    public class RepaymentService : IRepaymentService
    {
        private readonly BankLoanDbContext _context;

        public RepaymentService(BankLoanDbContext context)
        {
            _context = context;
        }

        public async Task<List<RepaymentScheduleViewModel>> GetRepaymentScheduleAsync(int applicationId)
        {
            return await _context.Repayments
                .Include(r => r.LoanApplication)
                    .ThenInclude(app => app.Customer)
                .Include(r => r.LoanApplication)
                    .ThenInclude(app => app.LoanProduct)
                .Include(r => r.Loan)
                .Where(r => r.ApplicationId == applicationId && r.Loan.DisburseStatus == "DISBURSED")
                .OrderBy(r => r.DueDate)
                .Select(r => new RepaymentScheduleViewModel
                {
                    RepaymentId = r.RepaymentId,
                    LoanId = r.LoanId,
                    ProductType = r.LoanApplication.LoanProduct.ProductType,
                    ProductName = r.LoanApplication.LoanProduct.ProductName,
                    DueDate = r.DueDate,
                    AmountDue = r.AmountDue,
                    PaymentDate = r.PaymentDate,
                    PaymentStatus = r.PaymentStatus
                })

                .ToListAsync();
        }





        public async Task GenerateScheduleAsync(int applicationId)
        {
            var loan = await _context.Loans
                .Include(l => l.LoanApplication)
                    .ThenInclude(app => app.LoanProduct)
                .FirstOrDefaultAsync(l => l.ApplicationId == applicationId && l.DisburseStatus == "DISBURSED");

            if (loan == null)
                throw new Exception("Loan not found or not disbursed.");

            var application = loan.LoanApplication;
            var product = application.LoanProduct;

            decimal principal = application.LoanAmount;
            int tenureMonths = application.LoanTenure;
            decimal annualRate = product.InterestRate;
            decimal tenureYears = (decimal)tenureMonths / 12;

            // Simple Interest Calculation
            decimal totalInterest = (principal * annualRate * tenureYears) / 100;
            decimal emi = (principal + totalInterest) / tenureMonths;
            emi = Math.Round(emi, 2);

            var startDate = loan.DisbursementDate ?? DateTime.Now;
            var repayments = new List<Repayment>();

            for (int i = 1; i <= tenureMonths; i++)
            {
                repayments.Add(new Repayment
                {
                    LoanId = loan.LoanId,
                    ApplicationId = applicationId,
                    DueDate = startDate.AddMonths(i),
                    AmountDue = emi,
                    PaymentStatus = "PENDING",
                });
            }

            await _context.Repayments.AddRangeAsync(repayments);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RepaymentScheduleViewModel>> GetLoansForSchedulingAsync()
        {
            return await _context.Loans
                .Include(l => l.LoanApplication)
                    .ThenInclude(app => app.Customer)
                .Include(l => l.LoanApplication)
                    .ThenInclude(app => app.LoanProduct)
                .Where(l => l.DisburseStatus == "DISBURSED")
                .Select(l => new RepaymentScheduleViewModel
                {
                    CustomerId = l.LoanApplication.Customer.CustomerId,
                    Name = l.LoanApplication.Customer.Name,
                    LoanId = l.LoanId,
                    ProductType = l.LoanApplication.LoanProduct.ProductType,
                    ProductName = l.LoanApplication.LoanProduct.ProductName,
                    InterestRate = l.LoanApplication.LoanProduct.InterestRate,
                    LoanAmount = l.LoanApplication.LoanAmount,
                    LoanTenure = l.LoanApplication.LoanTenure,
                    ApplicationId = l.ApplicationId
                })
                .ToListAsync();
        }
        public async Task<RepaymentScheduleViewModel> MakePaymentAsync(int repaymentId)
        {
            var repayment = await _context.Repayments
                .Include(r => r.Loan)
                .ThenInclude(l => l.LoanApplication)
                .ThenInclude(app => app.LoanProduct)
                .Include(r => r.LoanApplication)
                .ThenInclude(app => app.Customer)
                .FirstOrDefaultAsync(r => r.RepaymentId == repaymentId);

            if (repayment == null)
                throw new Exception("Repayment not found.");

            if (repayment.PaymentStatus == "COMPLETED")
                throw new Exception("This repayment is already completed.");

            Console.WriteLine("[DEBUG] Updating repayment record...");
            var loan = repayment.Loan;

            int paidCount = await _context.Repayments
                .CountAsync(r => r.LoanId == loan.LoanId && r.PaymentStatus == "COMPLETED");

            repayment.PaidAmount = repayment.AmountDue;
            repayment.AmountDue = 0;
            repayment.PaymentDate = DateTime.Now;
            repayment.PaymentStatus = "COMPLETED";

            // Use original principal for first payment, else use current outstanding balance
            var pr = paidCount == 0
                ? loan.LoanApplication.LoanAmount
                : loan.OutstandingBalance;

            var rate = loan.LoanApplication.LoanProduct.InterestRate;
            var tenureMonths = loan.LoanApplication.LoanTenure;
            var tenureYears = (decimal)tenureMonths / 12;

            // Simple Interest
            decimal totalInterest = (pr * rate * tenureYears) / 100;
            decimal emi = (pr + totalInterest) / tenureMonths;

            // Reduce principal by principal component of EMI
            decimal monthlyInterest = (pr * rate) / (12 * 100);
            decimal principalComponent = emi - monthlyInterest;
            decimal newBalance = pr - principalComponent;

            loan.OutstandingBalance = Math.Round(newBalance, 2);

            Console.WriteLine($"[DEBUG] Saving changes to database. PaidCount: {paidCount}, New OutstandingBalance: {loan.OutstandingBalance}");

            _context.Repayments.Update(repayment);
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();

            Console.WriteLine("[DEBUG] Payment successfully saved.");

            return new RepaymentScheduleViewModel
            {
                RepaymentId = repayment.RepaymentId,
                LoanId = loan.LoanId,
                ApplicationId = repayment.ApplicationId,
                ProductType = loan.LoanApplication.LoanProduct.ProductType,
                ProductName = loan.LoanApplication.LoanProduct.ProductName,
                Name = loan.LoanApplication.Customer.Name,
                DueDate = repayment.DueDate,
                AmountDue = repayment.AmountDue,
                PaidAmount = (decimal)repayment.PaidAmount,
                PaymentDate = repayment.PaymentDate,
                PaymentStatus = repayment.PaymentStatus,
                InterestRate = rate,
                LoanAmount = loan.LoanApplication.LoanAmount,
                LoanTenure = loan.LoanApplication.LoanTenure,
                OutstandingBalance = loan.OutstandingBalance,
                CustomerId = loan.LoanApplication.Customer.CustomerId
            };
        }

        public async Task<decimal?> GetOutstandingBalanceAsync(int loanId)
        {
            var loan = await _context.Loans
                .Where(l => l.LoanId == loanId)
                .Select(l => (decimal?)l.OutstandingBalance)
                .FirstOrDefaultAsync();

            return loan;
        }


    }
}
