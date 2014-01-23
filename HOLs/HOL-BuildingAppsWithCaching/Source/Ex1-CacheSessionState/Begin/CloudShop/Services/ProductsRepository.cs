namespace CloudShop.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudShop.Models;

    public class ProductsRepository : IProductRepository
    {
        public List<string> GetProducts()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var query = from product in context.Products
                            select product.ProductName;
                return query.ToList();
            }
        }
    }
}