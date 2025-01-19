using BusinessLogic.IServices;
using DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
   

    public class ReportsService : IReportsService
    {
        private readonly EFContext _context;

        public ReportsService(EFContext context)
        {
            _context = context;
        }

        public async Task<SalesAggregatedReport> GetAggregatedSalesReportAsync()
        {
            var aggregatedData = await _context.SalesOrderDetails
                .GroupBy(s => new
                {
                    s.SalesOrder.OrderDate.Year,
                    s.SalesOrder.OrderDate.Month,
                    s.SpecialOfferProduct.Product.ProductSubcategoryId,
                    s.ProductId,
                    s.SalesOrder.CustomerId,
                })
                .Select(g => new AggregatedSalesData
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    CategoryId = g.Key.ProductSubcategoryId,
                    ProductId = g.Key.ProductId,
                    CustomerId = g.Key.CustomerId,
                    TotalSales = g.Sum(s => s.LineTotal)
                })
                .OrderByDescending(a => a.TotalSales) // Order by TotalSales descending
                .Take(10) // Take the top 10 records
                .ToListAsync();

            return new SalesAggregatedReport
            {
                AggregatedData = aggregatedData
            };
        }


        public async Task<List<CustomerSalesReport>> GetTopCustomersBySalesAsync()
        {
            return await _context.SalesOrderDetails
                .GroupBy(s => s.SalesOrder.CustomerId)
                .Select(g => new CustomerSalesReport
                {
                    CustomerId = g.Key,
                    TotalSales = g.Sum(s => s.LineTotal)
                })
                .OrderByDescending(r => r.TotalSales)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<YearlyCustomerSalesReport>> GetTopCustomersByYearlySalesAsync()
        {
            return await _context.SalesOrderDetails
                .GroupBy(s => new { s.SalesOrder.CustomerId, s.SalesOrder.OrderDate.Year })
                .Select(g => new YearlyCustomerSalesReport
                {
                    CustomerId = g.Key.CustomerId,
                    Year = g.Key.Year,
                    TotalSales = g.Sum(s => s.LineTotal)
                })
                .OrderByDescending(r => r.TotalSales)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<ProductSalesReport>> GetTopProductsBySalesAsync()
        {
            return await _context.SalesOrderDetails
                .GroupBy(s => s.ProductId)
                .Select(g => new ProductSalesReport
                {
                    ProductId = g.Key,
                    TotalSales = g.Sum(s => s.LineTotal)
                })
                .OrderByDescending(r => r.TotalSales)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<ProductProfitReport>> GetTopProductsByProfitAsync()
        {
            return await _context.SalesOrderDetails
                .GroupBy(s => s.ProductId)
                .Select(g => new ProductProfitReport
                {
                    ProductId = g.Key,
                    TotalProfit = g.Sum(s => (s.UnitPrice - s.SpecialOfferProduct.Product.StandardCost) * s.OrderQty)
                })
                .OrderByDescending(r => r.TotalProfit)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<YearlyProductSalesReport>> GetTopProductsByYearlySalesAsync()
        {
            return await _context.SalesOrderDetails
                .GroupBy(s => new { s.ProductId, s.SalesOrder.OrderDate.Year })
                .Select(g => new YearlyProductSalesReport
                {
                    ProductId = g.Key.ProductId,
                    Year = g.Key.Year,
                    TotalSales = g.Sum(s => s.LineTotal)
                })
                .OrderByDescending(r => r.TotalSales)
                .Take(10)
                .ToListAsync();
        }
    }
}
