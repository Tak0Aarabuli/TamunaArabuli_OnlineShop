using Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.IServices
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDropdownItem>> GetAllCustomersAsync();
    }
}
