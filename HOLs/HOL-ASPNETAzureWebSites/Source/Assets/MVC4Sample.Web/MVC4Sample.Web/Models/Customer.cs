using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC4Sample.Web.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Company { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string Image { get; set; }
    }
}