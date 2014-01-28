using System.Collections.Generic;
using System.Linq;
using CloudShop.Models;

namespace CloudShop.Services
{
    public class ProductsRepository : IProductRepository
    {
        public List<string> GetProducts()
        {
            List<string> products = null;

            AdventureWorksEntities context = new AdventureWorksEntities();
            var query = from product in context.Products
                        select product.Name;
            products = query.ToList();

            return products;
        }

        public List<string> Search(string criteria)
        {
            var context = new AdventureWorksEntities();
            var query = context.ExecuteStoreQuery<string>("Select Name from Production.Product Where Freetext(*,{0})", 
                criteria);

            return query.ToList();
        }

    }
}