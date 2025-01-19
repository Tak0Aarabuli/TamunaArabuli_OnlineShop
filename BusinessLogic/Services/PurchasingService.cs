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
    public class PurchasingService:IPurchasingService
    {
        private readonly EFContext _context;

        public PurchasingService(EFContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShipMethodDropdownItem>> GetAllShipMethodsAsync()
        {
            return await _context.ShipMethods.Take(10)
                .Select(c => new ShipMethodDropdownItem
                {
                    Id = c.ShipMethodId,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
