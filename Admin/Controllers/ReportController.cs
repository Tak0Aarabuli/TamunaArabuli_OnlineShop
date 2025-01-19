using BusinessLogic.IServices;
using Microsoft.AspNetCore.Mvc;
using Models.Report;

namespace Admin.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportsService _reportsService;

        public ReportController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        public async Task<ActionResult> IndexAsync()
        {
            var viewModel = new AllReportsViewModel
            {
                AggregatedSalesReport = await _reportsService.GetAggregatedSalesReportAsync(),
                TopCustomersBySales = await _reportsService.GetTopCustomersBySalesAsync(),
                TopCustomersByYearlySales = await _reportsService.GetTopCustomersByYearlySalesAsync(),
                TopProductsBySales = await _reportsService.GetTopProductsBySalesAsync(),
                TopProductsByProfit = await _reportsService.GetTopProductsByProfitAsync(),
                TopProductsByYearlySales = await _reportsService.GetTopProductsByYearlySalesAsync()
            };
            return View(viewModel);
        }
        // Sales Amounts Aggregated Report
        public async Task<IActionResult> AggregatedSalesReport()
        {
            var report = await _reportsService.GetAggregatedSalesReportAsync();
            return View(report);
        }

        // Top 10 Customers by Sales Amount
        public async Task<IActionResult> TopCustomersBySales()
        {
            var report = await _reportsService.GetTopCustomersBySalesAsync();
            return View(report);
        }

        // Top 10 Customers by Sales Amount for Each Year
        public async Task<IActionResult> TopCustomersByYearlySales()
        {
            var report = await _reportsService.GetTopCustomersByYearlySalesAsync();
            return View(report);
        }

        // Top 10 Products by Sales Amount
        public async Task<IActionResult> TopProductsBySales()
        {
            var report = await _reportsService.GetTopProductsBySalesAsync();
            return View(report);
        }

        // Top 10 Products by Sales Profit
        public async Task<IActionResult> TopProductsByProfit()
        {
            var report = await _reportsService.GetTopProductsByProfitAsync();
            return View(report);
        }

        // Top 10 Products by Sales Amount for Each Year
        public async Task<IActionResult> TopProductsByYearlySales()
        {
            var report = await _reportsService.GetTopProductsByYearlySalesAsync();
            return View(report);
        }
    }
}
