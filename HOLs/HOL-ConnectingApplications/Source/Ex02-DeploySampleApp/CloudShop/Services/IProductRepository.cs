using System.Collections.Generic;

namespace CloudShop.Services
{
    public interface IProductRepository
    {
        List<string> GetProducts();
        List<string> Search(string criteria);
    }
}