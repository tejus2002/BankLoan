using BankLoanProject.Data;
using BankLoanProject.Models;
using BankLoanProject.Models.Entities;
using BankLoanProject.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Microsoft.Data.SqlClient; // For SqlException
using Microsoft.EntityFrameworkCore;
//using BankLoanProject.Filters;

namespace BankLoanProject.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BankLoanDbContext _context;
        private readonly IWebHostEnvironment _env;
        
        private readonly ICustomerService _customerService;

        public CustomerController(BankLoanDbContext context, ICustomerService customerService, IWebHostEnvironment env)
        {
            _context = context;
            _customerService = customerService;
            _env = env;
        }


        private void PopulateIdentityTypes()
        {
            ViewBag.IdentityTypes = new SelectList(new[]
            {
        "PAN Card",
        "Passport",
        "Driving License",
        "Aadhar Card"
    });
        }
        public IActionResult CustomerRegistration()
        {
            PopulateIdentityTypes();
            return View();
        }

        

        public IActionResult CustomerProfile()
        {
            return View();
        
        }
        [HttpPost]
        
public async Task<IActionResult> CustomerRegistration(CustomerViewModel model)
    {
        Console.WriteLine("CustomerRegistration method started.");

        PopulateIdentityTypes();
        Console.WriteLine("Identity types populated.");

        if (ModelState.IsValid)
        {
            Console.WriteLine("Model state is valid.");

            try
            {
                var folderPath = Path.Combine(_env.WebRootPath, "uploads", model.Email);
                Console.WriteLine($"Creating directory at: {folderPath}");
                Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, model.IdentityProof.FileName);
                Console.WriteLine($"Saving file to: {filePath}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.IdentityProof.CopyToAsync(stream);
                    Console.WriteLine("File uploaded successfully.");
                }

                var customer = new Customer
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    
                    Address = model.Address,
                    KycStatus = "PENDING",
                    Password = model.Password,
                    Role="Customer",
                    DateOfBirth = model.DateOfBirth,
                    
                    DocumentPath = $"/uploads/{model.Email}/{model.IdentityProof.FileName}",
                    IdentityType = model.IdentityType,


                };

                _context.Customers.Add(customer);
                Console.WriteLine("Customer added to context.");

                await _context.SaveChangesAsync();
                Console.WriteLine("Changes saved to database.");

                ModelState.Clear();
                ViewBag.Success = "Customer registered successfully!";
                return View(new CustomerViewModel());
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.InnerException?.Message ?? dbEx.Message}");
                
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL error: {sqlEx.Message}");
                ViewBag.Error = "A SQL error occurred during registration.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General exception: {ex.Message}");
                ViewBag.Error = "An unexpected error occurred during registration.";
            }

            return View(model);
        }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }

            }

            Console.WriteLine("Model state is invalid.");
        ViewBag.Error = "Please fill all fields and upload a document.";
        return View(model);
    }


    public async Task<IActionResult> CustomerTable()
        {
            var customers = await _context.Customers.ToListAsync();
            return View(customers); // Pass as model, not ViewBag
        }






        //[SessionAuthorize("Admin")]

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new EditCustomerViewModel
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                DateOfBirth = customer.DateOfBirth,
                Address = customer.Address,
                IdentityType = customer.IdentityType,
                KycStatus = customer.KycStatus
            };

            return View(model);
        }


        //[SessionAuthorize("Admin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = await _context.Customers.FindAsync(model.CustomerId);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                existingCustomer.Name = model.Name;
                existingCustomer.Email = model.Email;
                existingCustomer.Phone = model.Phone;
                existingCustomer.DateOfBirth = model.DateOfBirth;
                existingCustomer.Address = model.Address;
                existingCustomer.IdentityType = model.IdentityType;
                existingCustomer.KycStatus = model.KycStatus;

                await _context.SaveChangesAsync();
                return RedirectToAction("CustomerTable");
            }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }

            }

            return View(model);
        }





        //[SessionAuthorize("Admin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.DeleteAsync(id);
            return RedirectToAction("CustomerTable");
        }



        //[SessionAuthorize("Admin")]

        [HttpPost]
        public async Task<IActionResult> Verify(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.KycStatus = "Approved";
            await _context.SaveChangesAsync();
            return RedirectToAction("CustomerTable");
        }
    }
}
