using DataAccess.EF;
using Models.Order;
using Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.IServices
{
    public interface ISalesOrderHeaderService
    {
        Task<PaginatedResult<SalesOrderHeaderModel>> GetAllOrderHeadersAsync(int page, int pageSize);
        Task<SalesOrderHeaderModel> GetOrderHeaderByIdAsync(int orderId);
        Task CreateOrderHeaderAsync(SalesOrderHeaderModel order);
        Task UpdateOrderHeaderAsync(SalesOrderHeaderModel order);
        Task DeleteOrderHeaderAsync(int orderId);
    }

}
