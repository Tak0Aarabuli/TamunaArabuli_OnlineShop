using BusinessLogic.IServices;
using DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly EFContext _context;

        public CustomerService(EFContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerDropdownItem>> GetAllCustomersAsync()
        {
            return await _context.Customers.OrderByDescending(p => p.CustomerId).Take(10)
                .Select(c => new CustomerDropdownItem
                {
                    Id = c.CustomerId,
                    Name = c.Person.FirstName +" "+ c.Person.LastName,
                })
                .ToListAsync();
        }
    }
}
