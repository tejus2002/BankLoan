//using BankLoanProject.Filters;
using BankLoanProject.Models;
using BankLoanProject.Services.Interface;
using BankLoanProject.Services.ModuleService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankLoanProject.Controllers
{
    public class DisbursementController : Controller
    {

        private readonly IDisbursementService _disbursementService;

        public DisbursementController(IDisbursementService loanService)
        {
            _disbursementService = loanService;
        }

        //[SessionAuthorize("Admin")]
        public IActionResult DisburseLoan()
        {
            int customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;
            var role = HttpContext.Session.GetString("Role");
            Console.WriteLine($"[DEBUG] Role: {role}, CustomerId: {customerId}");


            if (role == "Customer")
            {

                ViewBag.ApplicationList = _disbursementService.GetApprovedApplications(customerId);
            }
            else
            {
                ViewBag.ApplicationList = _disbursementService.GetAllApprovedApplications();
            }


            return View(new LoanApplicationViewModel());
        }

        [HttpPost]
        public IActionResult Disburse(int applicationId)
        {
            _disbursementService.Disburse(applicationId);
            return RedirectToAction("DisburseLoan");
        }


    }
}