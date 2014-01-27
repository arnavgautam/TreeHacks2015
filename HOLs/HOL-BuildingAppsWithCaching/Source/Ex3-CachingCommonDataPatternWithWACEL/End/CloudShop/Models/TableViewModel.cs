namespace CloudShop.Models
{
    using System.Collections.Generic;

    public class TableViewModel
    {
        public List<Customer> Customers { get; set; }

        public long ElapsedTime { get; set; }
    }
}