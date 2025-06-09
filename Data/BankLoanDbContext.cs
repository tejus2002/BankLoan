using BankLoanProject.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankLoanProject.Data
{
    public class BankLoanDbContext:DbContext
    {
        public BankLoanDbContext(DbContextOptions<BankLoanDbContext> options)
        : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<LoanProduct> LoanProducts { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Repayment> Repayments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Repayment>()
                .HasOne(r => r.LoanApplication)
                .WithMany()
                .HasForeignKey(r => r.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<Repayment>()
                .HasOne(r => r.Loan)
                .WithMany(l => l.Repayments)
                .HasForeignKey(r => r.LoanId)
                .OnDelete(DeleteBehavior.Cascade); // Allow cascade from Loan

        }




    }

}
