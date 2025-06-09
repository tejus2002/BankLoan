using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankLoanProject.Models
{
    public class LoanApplicationViewModel
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }

       
        public string ProductType { get; set; }


        
        public int LoanProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? InterestRate { get; set; }
        public string? ApprovalStatus { get; set; }

        
        public decimal LoanAmount { get; set; }

        
        public int LoanTenure { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ApplicationDate { get; set; }

        public List<SelectListItem> ProductTypes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ProductNames { get; set; } = new List<SelectListItem>();
        public int? ApplicationId { get; set; }
    }
}
