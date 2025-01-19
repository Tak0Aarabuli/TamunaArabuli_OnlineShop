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
    public class SalesOrderHeaderService : ISalesOrderHeaderService
    {
        private readonly EFContext _context;

        public SalesOrderHeaderService(EFContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<SalesOrderHeaderModel>> GetAllOrderHeadersAsync(int page, int pageSize)
        {
            // Calculate total records
            var totalRecords = await _context.SalesOrderHeaders.CountAsync();

            // Fetch paginated data
            var salesOrderHeaders = await _context.SalesOrderHeaders
                .Include(soh => soh.Customer)
                .OrderByDescending(soh => soh.SalesOrderId) // Ensure consistent ordering
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(soh => new SalesOrderHeaderModel
                {
                    SalesOrderId = soh.SalesOrderId,
                    RevisionNumber = soh.RevisionNumber,
                    OrderDate = soh.OrderDate,
                    DueDate = soh.DueDate,
                    ShipDate = soh.ShipDate,
                    Status = soh.Status,
                    OnlineOrderFlag = soh.OnlineOrderFlag,
                    PurchaseOrderNumber = soh.PurchaseOrderNumber,
                    AccountNumber = soh.AccountNumber,
                    CustomerId = soh.CustomerId,
                    CustomerName = soh.Customer.Person.FirstName + " " + soh.Customer.Person.LastName,
                    SubTotal = soh.SubTotal,
                    TaxAmt = soh.TaxAmt,
                    Freight = soh.Freight,
                    Comment = soh.Comment
                })
                .ToListAsync();

            // Return paginated result
            return new PaginatedResult<SalesOrderHeaderModel>
            {
                Items = salesOrderHeaders,
                TotalRecords = totalRecords,
                CurrentPage = page,
                PageSize = pageSize
            };
        }


        public async Task<SalesOrderHeaderModel?> GetOrderHeaderByIdAsync(int orderId)
        {
            return await _context.SalesOrderHeaders
                .Where(soh => soh.SalesOrderId == orderId)
                .Select(soh => new SalesOrderHeaderModel
                {
                    SalesOrderId = soh.SalesOrderId,
                    RevisionNumber = soh.RevisionNumber,
                    OrderDate = soh.OrderDate,
                    DueDate = soh.DueDate,
                    ShipDate = soh.ShipDate,
                    Status = soh.Status,
                    OnlineOrderFlag = soh.OnlineOrderFlag,
                    PurchaseOrderNumber = soh.PurchaseOrderNumber,
                    AccountNumber = soh.AccountNumber,
                    CustomerId = soh.CustomerId,
                    BillToAddressId = soh.BillToAddressId,
                    ShipToAddressId = soh.ShipToAddressId,
                    ShipMethodId = soh.ShipMethodId,
                    SubTotal = soh.SubTotal,
                    TaxAmt = soh.TaxAmt,
                    Freight = soh.Freight,
                    Comment = soh.Comment,
                    SalesOrderDetails = soh.SalesOrderDetails
                        .Select(detail => new SalesOrderDetailModel
                        {
                            SalesOrderDetailId = detail.SalesOrderDetailId,
                            SalesOrderId = detail.SalesOrderId,
                            CarrierTrackingNumber = detail.CarrierTrackingNumber,
                            OrderQty = detail.OrderQty,
                            ProductId = detail.ProductId,
                            SpecialOfferId = detail.SpecialOfferId,
                            UnitPrice = detail.UnitPrice,
                            UnitPriceDiscount = detail.UnitPriceDiscount,
                            LineTotal = detail.LineTotal,
                            Rowguid = detail.Rowguid,
                            ModifiedDate = detail.ModifiedDate
                        }).ToList() // Map details to a list
                })
                .FirstOrDefaultAsync();
        }



        public async Task CreateOrderHeaderAsync(SalesOrderHeaderModel orderDto)
        {
            if (orderDto == null)
            {
                throw new ArgumentNullException(nameof(orderDto), "Order data cannot be null.");
            }

            if (orderDto.OrderDate > orderDto.DueDate)
            {
                throw new ArgumentException("Order date cannot be later than the due date.");
            }

            if (orderDto.SubTotal <= 0)
            {
                throw new ArgumentException("Subtotal must be greater than 0.");
            }

            // Ensure required related entities exist
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == orderDto.CustomerId);
            if (!customerExists)
            {
                throw new KeyNotFoundException($"Customer with Id {orderDto.CustomerId} not found.");
            }

            var billToAddressExists = await _context.Addresses.AnyAsync(a => a.AddressId == orderDto.BillToAddressId);
            if (!billToAddressExists)
            {
                throw new KeyNotFoundException($"Billing address with Id {orderDto.BillToAddressId} not found.");
            }

            var shipToAddressExists = await _context.Addresses.AnyAsync(a => a.AddressId == orderDto.ShipToAddressId);
            if (!shipToAddressExists)
            {
                throw new KeyNotFoundException($"Shipping address with Id {orderDto.ShipToAddressId} not found.");
            }

            var shipMethodExists = await _context.ShipMethods.AnyAsync(s => s.ShipMethodId == orderDto.ShipMethodId);
            if (!shipMethodExists)
            {
                throw new KeyNotFoundException($"Shipping method with Id {orderDto.ShipMethodId} not found.");
            }

            // Map DTO to entity
            var order = new SalesOrderHeader
            {
                RevisionNumber = orderDto.RevisionNumber,
                OrderDate = orderDto.OrderDate,
                DueDate = orderDto.DueDate,
                ShipDate = orderDto.ShipDate,
                Status = orderDto.Status,
                OnlineOrderFlag = orderDto.OnlineOrderFlag,
                PurchaseOrderNumber = orderDto.PurchaseOrderNumber,
                AccountNumber = orderDto.AccountNumber,
                CustomerId = orderDto.CustomerId,
                BillToAddressId = orderDto.BillToAddressId,
                ShipToAddressId = orderDto.ShipToAddressId,
                ShipMethodId = orderDto.ShipMethodId,
                SubTotal = orderDto.SubTotal,
                TaxAmt = orderDto.TaxAmt,
                Freight = orderDto.Freight,
                Comment = orderDto.Comment,
                ModifiedDate = DateTime.UtcNow,
                Rowguid = Guid.NewGuid()
            };

            // Add order to the database
            try
            {
                _context.SalesOrderHeaders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to create sales order. See inner exception for details.", ex);
            }
        }


        public async Task UpdateOrderHeaderAsync(SalesOrderHeaderModel orderDto)
        {
            var order = await _context.SalesOrderHeaders.FindAsync(orderDto.SalesOrderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            order.RevisionNumber = orderDto.RevisionNumber;
            order.OrderDate = orderDto.OrderDate;
            order.DueDate = orderDto.DueDate;
            order.ShipDate = orderDto.ShipDate;
            order.Status = orderDto.Status;
            order.OnlineOrderFlag = orderDto.OnlineOrderFlag;
            order.PurchaseOrderNumber = orderDto.PurchaseOrderNumber;
            order.AccountNumber = orderDto.AccountNumber;
            order.CustomerId = orderDto.CustomerId;
            order.SubTotal = orderDto.SubTotal;
            order.TaxAmt = orderDto.TaxAmt;
            order.Freight = orderDto.Freight;
            order.Comment = orderDto.Comment;
            order.ModifiedDate = DateTime.Now;

            _context.SalesOrderHeaders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderHeaderAsync(int orderId)
        {
            var order = await _context.SalesOrderHeaders
                .Include(soh => soh.SalesOrderDetails) // Include details for cascade delete if not configured
                .FirstOrDefaultAsync(o => o.SalesOrderId == orderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            _context.SalesOrderHeaders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

}
