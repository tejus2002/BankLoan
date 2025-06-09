using BankLoanProject.Data;
using BankLoanProject.Models.Entities;
using BankLoanProject.Services.Interface;
namespace BankLoanProject.Services.ModuleService
{
    public class LoanProductService: ILoanProductService
    {
        private readonly BankLoanDbContext _context;

        public LoanProductService(BankLoanDbContext context)
        {
            _context = context;
        }

        public IEnumerable<LoanProduct> GetAll() => _context.LoanProducts.ToList();

        public LoanProduct GetById(int id) => _context.LoanProducts.Find(id);

        public void Add(LoanProduct product)
        {
            try
            {
                Console.WriteLine("Starting to add a new loan product...");
                Console.WriteLine("Product details: " + product.ToString());

                _context.LoanProducts.Add(product);
                Console.WriteLine("Product added to context.");

                _context.SaveChanges();
                Console.WriteLine("Changes saved to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the loan product.");
                Console.WriteLine("Error message: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
                throw;
            }
        }


        public void Update(LoanProduct product)
        {
            _context.LoanProducts.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.LoanProducts.Find(id);
            if (product != null)
            {
                _context.LoanProducts.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
