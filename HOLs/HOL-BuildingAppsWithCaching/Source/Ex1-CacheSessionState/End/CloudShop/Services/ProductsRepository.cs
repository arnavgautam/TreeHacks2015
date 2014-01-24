namespace CloudShop.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using CloudShop.Models;

    public class ProductsRepository : IProductRepository
    {
        public List<string> GetProducts()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var query = from product in context.Products
                            select product.ProductName;
                var products = query.ToList();
                return products;
            }
        }
    }
}