using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Report
{
    public class AllReportsViewModel
    {
        public SalesAggregatedReport AggregatedSalesReport { get; set; } = new();
        public List<CustomerSalesReport> TopCustomersBySales { get; set; } = new();
        public List<YearlyCustomerSalesReport> TopCustomersByYearlySales { get; set; } = new();
        public List<ProductSalesReport> TopProductsBySales { get; set; } = new();
        public List<ProductProfitReport> TopProductsByProfit { get; set; } = new();
        public List<YearlyProductSalesReport> TopProductsByYearlySales { get; set; } = new();
    }
    public class SalesAggregatedReport
    {
        public List<AggregatedSalesData> AggregatedData { get; set; } = new();
    }

    public class AggregatedSalesData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int? CategoryId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalSales { get; set; }
    }

    public class CustomerSalesReport
    {
        public int CustomerId { get; set; }
        public decimal TotalSales { get; set; }
    }

    public class YearlyCustomerSalesReport : CustomerSalesReport
    {
        public int Year { get; set; }
    }

    public class ProductSalesReport
    {
        public int ProductId { get; set; }
        public decimal TotalSales { get; set; }
    }

    public class ProductProfitReport
    {
        public int ProductId { get; set; }
        public decimal TotalProfit { get; set; }
    }

    public class YearlyProductSalesReport : ProductSalesReport
    {
        public int Year { get; set; }
    }
}
