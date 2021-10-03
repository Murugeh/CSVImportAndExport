using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class CustomerDetailsInfo
    {
        public List<Customer> CustomerInfo { get; set; }
        public PagerModel Pager { get; set; }
    }
}