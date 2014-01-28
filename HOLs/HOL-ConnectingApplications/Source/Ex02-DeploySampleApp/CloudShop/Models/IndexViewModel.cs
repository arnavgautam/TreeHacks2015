using System.Collections.Generic;

namespace CloudShop.Models
{
    public class IndexViewModel
    {
        public string SearchCriteria { get; set; }
        public IEnumerable<string> Products { get; set; }
    }
}