using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webApi.Models;

namespace webApi.DAL
{
    public class WebApiInMemoryDB
    {
        public virtual IList<Address> Addresses { get; set; }
        public virtual IList<Customer> Customers { get; set; }
        public virtual IList<Invoice> Invoices { get; set; }
        public virtual IList<LineItem> LineItems { get; set; }
        public WebApiInMemoryDB()
        {
            Addresses = new List<Address>();
            Customers = new List<Customer>();
            Invoices = new List<Invoice>();
            LineItems = new List<LineItem>();
        }
    }
}
