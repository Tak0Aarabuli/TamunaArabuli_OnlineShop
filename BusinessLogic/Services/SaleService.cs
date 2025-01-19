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
    public class SaleService:ISaleService
    {
        private readonly EFContext _context;

        public SaleService(EFContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SpecialOfferProductDropdownItem>> GetAllSpecialOfferProductsAsync()
        {
            return await _context.SpecialOfferProducts.Take(10)
                .Select(c => new SpecialOfferProductDropdownItem
                {
                    Id = c.SpecialOfferId + ","+c.ProductId,
                    Name = c.Product.Name,
                })
                .ToListAsync();
        }
    }
}
