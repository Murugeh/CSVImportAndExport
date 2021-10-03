using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.DataAccess.Abstraction.AbstractModels
{
    public interface ICustomerInfo
    {
         string CustomerName { get; set; }
         string City { get; set; }
         string State { get; set; }
         string Country { get; set; }
    }
}
