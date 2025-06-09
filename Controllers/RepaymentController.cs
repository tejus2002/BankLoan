//using BankLoanProject.Filters;
using BankLoanProject.Models;
using BankLoanProject.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BankLoanProject.Controllers
{
    public class RepaymentController : Controller
    {
        private readonly IRepaymentService _repaymentService;

        public RepaymentController(IRepaymentService repaymentService)
        {
            _repaymentService = repaymentService;
        }

        //[SessionAuthorize("Customer")]
        [HttpGet]
        public async Task<IActionResult> Customer()
        {


            int customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0; // Simulated session-based customer ID

            var allLoans = await _repaymentService.GetLoansForSchedulingAsync();

            var customerApplications = allLoans
            .Where(l => l.CustomerId == customerId)
            .Select(l => l.ApplicationId)
            .ToList();

            var allRepayments = new List<RepaymentScheduleViewModel>();
            foreach (var appId in customerApplications)
            {
                var repayments = await _repaymentService.GetRepaymentScheduleAsync(appId);
                allRepayments.AddRange(repayments);
            }

            return View("Customer", allRepayments); // View name matches the Razor file
        }
        //[SessionAuthorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> GenerateSchedule(int applicationId)
        {
            try
            {
                await _repaymentService.GenerateScheduleAsync(applicationId);
                TempData["Success"] = "Repayment schedule generated successfully!";
                return RedirectToAction("GetRepaymentSchedule", new { applicationId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Admin");
            }
        }
        //[SessionAuthorize("Admin")]
        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            var loans = await _repaymentService.GetLoansForSchedulingAsync();
            return View(loans);
        }
        //[SessionAuthorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> ScheduleLoan(int applicationId)
        {
            try
            {
                await _repaymentService.GenerateScheduleAsync(applicationId);
                TempData["Success"] = "Schedule generated successfully!";
            }
            catch (Exception ex)
            {

                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                TempData["Error"] = "Database error: " + innerMessage;

            }

            return RedirectToAction("Admin");
        }
        //[SessionAuthorize("Customer")]
        [HttpPost]
        public async Task<IActionResult> Pay(int repaymentId)
        {
            try
            {
                await _repaymentService.MakePaymentAsync(repaymentId);
                TempData["Success"] = "Payment successful!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Customer");
        }
        //[SessionAuthorize("Customer")]
        public async Task<IActionResult> GetOutstandingBalance(int loanId)
        {
            var balance = await _repaymentService.GetOutstandingBalanceAsync(loanId);
            if (balance == null)
            {
                TempData["Error"] = "Loan not found.";
                return RedirectToAction("Customer");
            }

            ViewBag.LoanId = loanId;
            ViewBag.OutstandingBalance = balance;

            // Load the model expected by the Customer view
            int customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0; // Simulated session-based customer ID
            var allLoans = await _repaymentService.GetLoansForSchedulingAsync();
            var customerApplications = allLoans
                .Where(l => l.CustomerId == customerId)
                .Select(l => l.ApplicationId)
                .ToList();

            var allRepayments = new List<RepaymentScheduleViewModel>();
            foreach (var appId in customerApplications)
            {
                var repayments = await _repaymentService.GetRepaymentScheduleAsync(appId);
                allRepayments.AddRange(repayments);
            }

            return View("Customer", allRepayments); // ✅ Pass the model here
        }
    }
}
