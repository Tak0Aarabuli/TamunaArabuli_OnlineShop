using Models.Customer;
using Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.IServices
{
    public interface IPersonService
    {
        Task<IEnumerable<AddressDropdownItem>> GetAllAddressesAsync();
    }
}
