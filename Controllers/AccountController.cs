using System.Diagnostics;
using System.Security.Claims;
using BankLoanProject.Data;
//using BankLoanProject.Filters;
using BankLoanProject.Models;
using BankLoanProject.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BankLoanProject.Controllers
{
    public class AccountController : Controller
    {
        
        public IActionResult Hero()
        {
            return View();
        }
        


        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        //[SessionAuthorize("Admin")]

        public IActionResult AdminLogin(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _accountService.ValidateAdminLogin(model);
                if (admin != null)
                {
                    HttpContext.Session.SetInt32("AdminId", admin.AdminId);
                    HttpContext.Session.SetString("Role", admin.Role);
                    return RedirectToAction("CustomerTable", "Customer");
                }

                ModelState.AddModelError("", "Invalid Admin ID or Password.");
            }

            return View(model);
        }


        //[SessionAuthorize("Customer")]

        public IActionResult CustomerLogin(CustomerLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = _accountService.ValidateCustomerLogin(model);
                if (customer != null)
                {
                    HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);
                    HttpContext.Session.SetString("Role", customer.Role);
                    HttpContext.Session.SetString("CustomerName", customer.Name ?? "");
                    return RedirectToAction("CustomerProfile", "Customer");
                }

                ModelState.AddModelError("", "Invalid Phone Number or Password.");
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clears all session data
            return RedirectToAction("Hero", "Account"); // Or redirect to your login page
        }


    }
}
