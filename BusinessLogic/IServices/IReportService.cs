using Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.IServices
{
    public interface IReportsService
    {
        Task<SalesAggregatedReport> GetAggregatedSalesReportAsync();
        Task<List<CustomerSalesReport>> GetTopCustomersBySalesAsync();
        Task<List<YearlyCustomerSalesReport>> GetTopCustomersByYearlySalesAsync();
        Task<List<ProductSalesReport>> GetTopProductsBySalesAsync();
        Task<List<ProductProfitReport>> GetTopProductsByProfitAsync();
        Task<List<YearlyProductSalesReport>> GetTopProductsByYearlySalesAsync();
    }
}
