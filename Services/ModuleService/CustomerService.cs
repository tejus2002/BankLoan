﻿using BankLoanProject.Data;
using BankLoanProject.Models.Entities;
using BankLoanProject.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BankLoanProject.Services.ModuleService
{
    public class CustomerService:ICustomerService
    {
        private readonly BankLoanDbContext _context;

        public CustomerService(BankLoanDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();

        public async Task<Customer?> GetByIdAsync(int id) => await _context.Customers.FindAsync(id);

        public async Task AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

    }
}
