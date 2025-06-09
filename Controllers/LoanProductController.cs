using BankLoanProject.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using BankLoanProject.Services.Interface;
//using BankLoanProject.Filters;
namespace BankLoanProject.Controllers
{
    public class LoanProductController : Controller
    {
        private readonly ILoanProductService _service;

        public LoanProductController(ILoanProductService service)
        {
            _service = service;
        }

        //[SessionAuthorize("Admin","Customer")]

        public IActionResult Index()
        {
            //ViewBag.LoanProducts = _service.GetAll();
            //return View();
            var products = _service.GetAll();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddLoanProduct()
        {
            return View();
        }

        //[SessionAuthorize("Admin")]

        [HttpPost]
        public IActionResult AddLoanProduct(LoanProduct product)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Validation error: " + error.ErrorMessage);
                }
                return View(product);
            }

            _service.Add(product);
            TempData["Success"] = "Loan Product added successfully!";
            return RedirectToAction("Index");
        }



        //[SessionAuthorize("Admin")]

        public IActionResult UpdateLoanProduct(int id)
        {
            var product = _service.GetById(id);
            if (product == null) return NotFound();

            // Return partial view for AJAX
            return PartialView("UpdateLoanProduct", product);
        }


        //[SessionAuthorize("Admin")]

        [HttpPost]
        public IActionResult UpdateLoanProduct(LoanProduct product)
        {
            if (ModelState.IsValid)
            {
                _service.Update(product);
                TempData["Success"] = "Loan Product updated successfully!";
                return RedirectToAction("Index");
            }
            ViewBag.Product = product;
            return View();
        }

        //[SessionAuthorize("Admin")]

        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            TempData["Success"] = "Loan Product deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}

