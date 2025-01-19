using BusinessLogic.IServices;
using DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Models.Customer;
using Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class PersonService:IPersonService
    {
        private readonly EFContext _context;

        public PersonService(EFContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AddressDropdownItem>> GetAllAddressesAsync()
        {
            return await _context.Addresses.Take(10)
                .Select(c => new AddressDropdownItem
                {
                    Id = c.AddressId,
                    Name = c.AddressLine1
                })
                .ToListAsync();
        }
    }
}
