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
    public interface ISalesOrderDetailService
    {
        Task<PaginatedResult<SalesOrderDetailModel>> GetAllOrderDetailsAsync(int page, int pageSize);
        Task<SalesOrderDetailModel> GetOrderDetailByIdAsync(int orderId);
        Task CreateOrderDetailAsync(SalesOrderDetailModel order);
        Task UpdateOrderDetailAsync(SalesOrderDetailModel order);
        Task DeleteOrderDetailAsync(int orderId);
    }

}
