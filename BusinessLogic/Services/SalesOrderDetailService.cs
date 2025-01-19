using BusinessLogic.IServices;
using DataAccess.EF;
using Models.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing.Printing;
using Models.Shared;

namespace BusinessLogic.Services
{
    public class SalesOrderDetailService : ISalesOrderDetailService
    {
        private readonly EFContext _context;

        public SalesOrderDetailService(EFContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<SalesOrderDetailModel>> GetAllOrderDetailsAsync(int page, int pageSize)
        {
            // Calculate total records for pagination
            var totalRecords = await _context.SalesOrderDetails.CountAsync();

            // Fetch paginated data
            var salesOrderDetails = await _context.SalesOrderDetails
                .OrderByDescending(sod => sod.SalesOrderDetailId) // Order by SalesOrderDetailId for consistent results
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(sod => new SalesOrderDetailModel
                {
                    SalesOrderId = sod.SalesOrderId,
                    SalesOrderDetailId = sod.SalesOrderDetailId,
                    CarrierTrackingNumber = sod.CarrierTrackingNumber,
                    OrderQty = sod.OrderQty,
                    ProductId = sod.ProductId,
                    SpecialOfferId = sod.SpecialOfferId,
                    UnitPrice = sod.UnitPrice,
                    UnitPriceDiscount = sod.UnitPriceDiscount,
                    LineTotal = sod.LineTotal,
                    Rowguid = sod.Rowguid,
                    ModifiedDate = sod.ModifiedDate
                })
                .ToListAsync();

            // Return paginated result
            return new PaginatedResult<SalesOrderDetailModel>
            {
                Items = salesOrderDetails,
                TotalRecords = totalRecords,
                CurrentPage = page,
                PageSize = pageSize
            };
        }


        public async Task<SalesOrderDetailModel?> GetOrderDetailByIdAsync(int salesOrderDetailId)
        {
            return await _context.SalesOrderDetails
                .Where(sod => sod.SalesOrderDetailId == salesOrderDetailId) // Filter by SalesOrderDetailId
                .Select(sod => new SalesOrderDetailModel
                {
                    SalesOrderId = sod.SalesOrderId,
                    SalesOrderDetailId = sod.SalesOrderDetailId,
                    CarrierTrackingNumber = sod.CarrierTrackingNumber,
                    OrderQty = sod.OrderQty,
                    ProductId = sod.ProductId,
                    SpecialOfferId = sod.SpecialOfferId,
                    UnitPrice = sod.UnitPrice,
                    UnitPriceDiscount = sod.UnitPriceDiscount,
                    LineTotal = sod.LineTotal,
                    Rowguid = sod.Rowguid,
                    ModifiedDate = sod.ModifiedDate
                })
                .FirstOrDefaultAsync();
        }



        public async Task CreateOrderDetailAsync(SalesOrderDetailModel orderDto)
        {
            if (orderDto == null)
            {
                throw new ArgumentNullException(nameof(orderDto), "Order data cannot be null.");
            }

            // ValIdate fields
            if (orderDto.OrderQty <= 0)
            {
                throw new ArgumentException("Order quantity must be greater than 0.");
            }

            if (orderDto.UnitPrice <= 0)
            {
                throw new ArgumentException("Unit price must be greater than 0.");
            }

            if (orderDto.UnitPriceDiscount < 0)
            {
                throw new ArgumentException("Unit price discount cannot be negative.");
            }

            if (orderDto.ProductId <= 0)
            {
                throw new ArgumentException("ValId Product Id is required.");
            }

            if (orderDto.SpecialOfferId <= 0)
            {
                throw new ArgumentException("ValId Special Offer Id is required.");
            }

            // Ensure related entities exist
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == orderDto.ProductId);
            if (!productExists)
            {
                throw new KeyNotFoundException($"Product with Id {orderDto.ProductId} not found.");
            }

            var specialOfferExists = await _context.SpecialOffers.AnyAsync(s => s.SpecialOfferId == orderDto.SpecialOfferId);
            if (!specialOfferExists)
            {
                throw new KeyNotFoundException($"Special offer with Id {orderDto.SpecialOfferId} not found.");
            }

            // Map DTO to entity
            var orderDetail = new SalesOrderDetail
            {
                SalesOrderId = orderDto.SalesOrderId,
                CarrierTrackingNumber = orderDto.CarrierTrackingNumber,
                OrderQty = orderDto.OrderQty,
                ProductId = orderDto.ProductId,
                SpecialOfferId = orderDto.SpecialOfferId,
                UnitPrice = orderDto.UnitPrice,
                UnitPriceDiscount = orderDto.UnitPriceDiscount,
                ModifiedDate = DateTime.UtcNow,
                Rowguid = Guid.NewGuid()
            };

            try
            {
                // Add order detail to the database
                _context.SalesOrderDetails.Add(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to create sales order detail. See inner exception for details.", ex);
            }
        }



        public async Task UpdateOrderDetailAsync(SalesOrderDetailModel orderDto)
        {
            if (orderDto == null)
            {
                throw new ArgumentNullException(nameof(orderDto), "Order data cannot be null.");
            }

            // Find the order detail record by SalesOrderDetailID
            var orderDetail = await _context.SalesOrderDetails.FirstOrDefaultAsync(sod => sod.SalesOrderDetailId == orderDto.SalesOrderDetailId);
            if (orderDetail == null)
            {
                throw new KeyNotFoundException($"Order detail with ID {orderDto.SalesOrderDetailId} not found.");
            }

            // Validate related entities
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == orderDto.ProductId);
            if (!productExists)
            {
                throw new KeyNotFoundException($"Product with ID {orderDto.ProductId} not found.");
            }

            var specialOfferExists = await _context.SpecialOffers.AnyAsync(s => s.SpecialOfferId == orderDto.SpecialOfferId);
            if (!specialOfferExists)
            {
                throw new KeyNotFoundException($"Special offer with ID {orderDto.SpecialOfferId} not found.");
            }

            // Update fields
            orderDetail.CarrierTrackingNumber = orderDto.CarrierTrackingNumber;
            orderDetail.OrderQty = orderDto.OrderQty;
            orderDetail.ProductId = orderDto.ProductId;
            orderDetail.SpecialOfferId = orderDto.SpecialOfferId;
            orderDetail.UnitPrice = orderDto.UnitPrice;
            orderDetail.UnitPriceDiscount = orderDto.UnitPriceDiscount;
            orderDetail.ModifiedDate = DateTime.UtcNow;

            try
            {
                // Update order detail in the database
                _context.SalesOrderDetails.Update(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to update sales order detail. See inner exception for details.", ex);
            }
        }


        public async Task DeleteOrderDetailAsync(int orderDetailId)
        {
            // Find the specific order detail by its ID
            var orderDetail = await _context.SalesOrderDetails.FirstOrDefaultAsync(sod => sod.SalesOrderDetailId == orderDetailId); 

            // If the order detail is not found, throw an exception
            if (orderDetail == null)
            {
                throw new KeyNotFoundException($"Order detail with ID {orderDetailId} not found.");
            }

            try
            {
                // Remove the order detail from the database
                _context.SalesOrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle any database-related exceptions gracefully
                throw new InvalidOperationException("Failed to delete order detail. See inner exception for details.", ex);
            }
        }

    }

}
