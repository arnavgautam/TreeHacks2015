namespace CloudShop.Models
{
    using System;

    public class Customer
    {
        public string Id { get; set; }

        public string Company { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public string Comment { get; set; }

        public DateTime ContractDate { get; set; }
    }
}