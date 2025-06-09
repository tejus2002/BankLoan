//using BankLoanProject.Filters;
using BankLoanProject.Models;
using BankLoanProject.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BankLoanProject.Controllers
{
    //[SessionAuthorize("Admin", "Customer")]
    public class ReportController : Controller
    {
        

        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Report()
        {
            return View();
        }
        public IActionResult LoanReport()
        {
            int customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0; // You should have this method implemented
            var role = HttpContext.Session.GetString("Role");
            Console.WriteLine($"[DEBUG] Role: {role}, CustomerId: {customerId}");

            List<LoanReportViewModel> data;

            if (role == "Customer")
            {
                data = _reportService.GenerateLoanReportByCustomerId(customerId);
            }
            else
            {
                data = _reportService.GenerateLoanReport();
            }

            return View(data);
        }


        public IActionResult OutstandingReport()
        {
            int customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;
            var role = HttpContext.Session.GetString("Role");
            Console.WriteLine($"[DEBUG] Role: {role}, CustomerId: {customerId}");

            List<OutstandingReportViewModel> data;

            if (role == "Customer")
            {
                data = _reportService.GenerateOutstandingReportByCustomerId(customerId);
            }
            else
            {
                data = _reportService.GenerateOutstandingReport();
            }

            return View(data);
        }


        public IActionResult RepaymentReport()
        {
            int customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;
            var role = HttpContext.Session.GetString("Role");
            Console.WriteLine($"[DEBUG] Role: {role}, CustomerId: {customerId}");

            List<RepaymentReportViewModel> data;

            if (role == "Customer")
            {
                data = _reportService.GenerateRepaymentReportByCustomerId(customerId);
            }
            else
            {
                data = _reportService.GenerateRepaymentReport();
            }

            return View(data);
        }



    }
}
