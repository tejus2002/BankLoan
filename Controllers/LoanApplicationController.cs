using System.Diagnostics;
using BankLoanProject.Models;
using BankLoanProject.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using BankLoanProject.Models.Entities;
using System.Data;
//using BankLoanProject.Filters;
using Microsoft.EntityFrameworkCore;
namespace BankLoanProject.Controllers
{
    public class LoanApplicationController : Controller
    {
        private readonly ILoanApplicationService _loanService;

        public LoanApplicationController(ILoanApplicationService loanService)
        {
            _loanService = loanService;
        }
        //[SessionAuthorize("Admin", "Customer")]
        public IActionResult Application()
        {
            int customerId = GetCustomerIdFromSession();
            var role = HttpContext.Session.GetString("Role");
            //Console.WriteLine($"[DEBUG] Role: {role}, CustomerId: {customerId}");


            if (role == "Customer")
            {

                ViewBag.ApplicationList = _loanService.GetApplications(customerId);
            }
            else
            {
                ViewBag.ApplicationList = _loanService.GetAllApplications();
            }
            ViewBag.ProductTypes = _loanService.GetProductTypes();

            return View(new LoanApplicationViewModel());
        }


        //[SessionAuthorize("Customer")]

        [HttpPost]
        public IActionResult ApplyForLoan(LoanApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                _loanService.ApplyForLoan(model, GetCustomerIdFromSession());
                TempData["Success"] = "Loan Application Submitted!";
            }
            else
            {

                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state.Errors.Any())
                    {
                        Debug.WriteLine($"Field '{key}':");
                        foreach (var error in state.Errors)
                        {
                            Debug.WriteLine($"  - {error.ErrorMessage}");
                        }
                    }
                }

            }
            return RedirectToAction("Application");
            }
        

        [HttpGet]
        public JsonResult GetProductNamesByType(string productType)
        {
            var productNames = _loanService.GetProductNamesByType(productType);
            return Json(productNames);
        }

        private int GetCustomerIdFromSession()
        {
            return HttpContext.Session.GetInt32("CustomerId") ?? 0;

        }


        //[SessionAuthorize("Admin")]
        [HttpPost]
        public JsonResult UpdateStatus([FromBody] StatusUpdateModel model)
        {
            if (model != null)
            {
                _loanService.UpdateApplicationStatus(model.ApplicationId, model.Status);
                return Json(new { success = true, message = "Status updated successfully." });
            }
            return Json(new { success = false, message = "Invalid data." });
        }
        //[SessionAuthorize("Admin", "Customer")]
        [HttpGet]
        
        public JsonResult GetApplicationDetails(int customerId)
        {
            var application = _loanService.GetApplications(customerId).FirstOrDefault();

            if (application != null)
            {
                int applicationId = _loanService.GetApplicationIdByCustomerId(customerId);

                return Json(new
                {
                    applicationId = applicationId,
                    customerId = application.CustomerId,
                    name = application.Name,
                    productType = application.ProductType,
                    productName = application.ProductName,
                    interestRate = application.InterestRate,
                    loanTenure = application.LoanTenure,
                    loanAmount = application.LoanAmount,
                    applicationDate = application.ApplicationDate.ToString("dd-MM-yyyy"),
                    approvalStatus = application.ApprovalStatus
                });
            }

            return Json(null);
        }



    }









}

