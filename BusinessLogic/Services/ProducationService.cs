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
    public class ProducationService:IProducationService
    {
        private readonly EFContext _context;

        public ProducationService(EFContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDropdownItem>> GetAllProductsAsync()
        {
            return await _context.Products.Take(10)
                .Select(c => new ProductDropdownItem
                {
                    Id = c.ProductId,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
