using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.IServices;
using Models.Order;
using Models.Shared;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Customer;
using Microsoft.AspNetCore.Authorization;
namespace Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SalesOrderHeaderController : Controller
        {
            private readonly ISalesOrderHeaderService _salesOrderHeaderService;
            private readonly ISalesOrderDetailService _salesOrderDetailService;
            private readonly ICustomerService _customerService;
            private readonly IPersonService _personService;
            private readonly IPurchasingService _purchasingService;
            private readonly IProducationService _producationService;
            private readonly ISaleService _saleService;

            public SalesOrderHeaderController(ISalesOrderHeaderService salesOrderHeaderService, ISalesOrderDetailService salesOrderDetailService,
                ICustomerService customerService, IPersonService personService, 
                IPurchasingService purchasingService, IProducationService producationService,
                ISaleService saleService)
            {
                _salesOrderHeaderService = salesOrderHeaderService;
                _salesOrderDetailService = salesOrderDetailService;
                _customerService = customerService;
                _personService = personService;
                _purchasingService = purchasingService;
                _producationService = producationService;
                _saleService = saleService;
            }

        // GET: SalesOrder
            public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
            {
            var paginatedResult = await _salesOrderHeaderService.GetAllOrderHeadersAsync(page, pageSize);


            return View( paginatedResult); // Assumes a corresponding Razor view
        }

        // GET: SalesOrder/Details/5
        public async Task<IActionResult> Details(int id)
            {
                var order = await _salesOrderHeaderService.GetOrderHeaderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return View(order); // Assumes a corresponding Details.cshtml view
            }

            // GET: SalesOrder/Create
            public async Task<IActionResult> CreateAsync()
            {
                ViewBag.Customers =  CustomerSelectListAsync();
                ViewBag.Addresses =  AddressesSelectListAsync();
                ViewBag.ShipMethods =  ShipMethodsSelectListAsync();

                return View("AddSalesOrderHeader", new SalesOrderHeaderModel()); // Assumes a corresponding Create.cshtml view
             }

            // POST: SalesOrder/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(SalesOrderHeaderModel orderDto)
            {
                if (ModelState.IsValid)
                {
                    await _salesOrderHeaderService.CreateOrderHeaderAsync(orderDto);
                    TempData["Success"] = "Order created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            ViewBag.Customers =  CustomerSelectListAsync();
            ViewBag.Addresses =  AddressesSelectListAsync();
            ViewBag.ShipMethods =  ShipMethodsSelectListAsync();
            TempData["Error"] = "Failed to create order. Please check the input.";
                return View("AddSalesOrderHeader",orderDto);
            }

            // GET: SalesOrder/Edit/5
            public async Task<IActionResult> Edit(int id)
            {
                var order = await _salesOrderHeaderService.GetOrderHeaderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
            ViewBag.Customers =  CustomerSelectListAsync();
            ViewBag.Addresses =  AddressesSelectListAsync();
            ViewBag.ShipMethods =  ShipMethodsSelectListAsync();
            return View("EditSalesOrderHeader",order); // Assumes a corresponding Edit.cshtml view
            }

            // POST: SalesOrder/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(SalesOrderHeaderModel orderDto)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        await _salesOrderHeaderService.UpdateOrderHeaderAsync(orderDto);
                        TempData["Success"] = "Order updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (KeyNotFoundException)
                    {
                        return NotFound();
                    }
                }
            ViewBag.Customers =  CustomerSelectListAsync();
            ViewBag.Addresses =  AddressesSelectListAsync();
            ViewBag.ShipMethods =  ShipMethodsSelectListAsync();
            TempData["Error"] = "Failed to update order. Please check the input.";
                return View("EditSalesOrderHeader",orderDto);
            }


            // POST: SalesOrder/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Delete(int orderId)
            {
                try
                {
                    await _salesOrderHeaderService.DeleteOrderHeaderAsync(orderId);
                    TempData["Success"] = "Order deleted successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception)
                {
                    TempData["Error"] = "Failed to delete order. It might have dependent details.";
                    return RedirectToAction(nameof(Index));
                }
            }

        private  IEnumerable<SelectListItem> CustomerSelectListAsync()
        {
            return  _customerService.GetAllCustomersAsync().Result.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            }).ToList(); ; ;

        }

        private  IEnumerable<SelectListItem> AddressesSelectListAsync()
        {
            return  _personService.GetAllAddressesAsync().Result.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            }).ToList(); ;

        }
        private  IEnumerable<SelectListItem> ShipMethodsSelectListAsync()
        {
            return _purchasingService.GetAllShipMethodsAsync().Result.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            }).ToList(); ;

        }
        private IEnumerable<SelectListItem> SpecialOfferProductSelectList()
        {
            return _saleService.GetAllSpecialOfferProductsAsync().Result.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            }).ToList(); ;

        }


        // GET: SalesOrder/Create
        public IActionResult CreateDetail(int OrderHeaderId)
        {
            ViewBag.SpecialOffers = SpecialOfferProductSelectList();
            var sales = new SalesOrderDetailModel();
            sales.SalesOrderId = OrderHeaderId;
            return View("AddSalesOrderDetail", sales); // Assumes a corresponding Create.cshtml view
        }

        // POST: SalesOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDetail(SalesOrderDetailModel orderDto)
        {
            if (ModelState.IsValid)
            {
                string[] val = orderDto.val.Split(',');
                orderDto.SpecialOfferId = Convert.ToInt32(val[0]);
                orderDto.ProductId = Convert.ToInt32(val[1]);
                await _salesOrderDetailService.CreateOrderDetailAsync(orderDto);
                TempData["Success"] = "Order created successfully.";
                return RedirectToAction("Details", new { id = orderDto.SalesOrderId });
            }
            ViewBag.SpecialOffers = SpecialOfferProductSelectList();
            TempData["Error"] = "Failed to create order. Please check the input.";
            return View("AddSalesOrderDetail", orderDto);
        }

        public async Task<IActionResult> EditDetail(int id)
        {
            var order = await _salesOrderDetailService.GetOrderDetailByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.val=order.SpecialOfferId+","+order.ProductId;
            ViewBag.SpecialOffers = SpecialOfferProductSelectList();
            return View("EditSalesOrderDetail", order); // Assumes a corresponding Edit.cshtml view
        }

        // POST: SalesOrder/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetail(SalesOrderDetailModel orderDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string[] val = orderDto.val.Split(',');
                    orderDto.SpecialOfferId = Convert.ToInt32(val[0]);
                    orderDto.ProductId = Convert.ToInt32(val[1]);
                    await _salesOrderDetailService.UpdateOrderDetailAsync(orderDto);
                    TempData["Success"] = "Order updated successfully.";
                    return RedirectToAction("Details", new { id = orderDto.SalesOrderId });
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
            }
            ViewBag.SpecialOffers = SpecialOfferProductSelectList();
            TempData["Error"] = "Failed to update order. Please check the input.";
            return View("EditSalesOrderDetail", orderDto);
        }

        // POST: SalesOrder/Delete/5
        [HttpPost, ActionName("DeleteDetail")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDetail(int orderId)
        {
            try
            {
                await _salesOrderDetailService.DeleteOrderDetailAsync(orderId);
                TempData["Success"] = "Order deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                TempData["Error"] = "Failed to delete order. It might have dependent details.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
    }


