namespace FabrikamInsurance.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public static class Helpers
    {
        public static IEnumerable<SelectListItem> AsSelectList(this IEnumerable<KeyValuePair<string, string>> list)
        {
            return from item in list
                   select new SelectListItem() { Value = item.Key, Text = item.Value };
        }

        public static IEnumerable<SelectListItem> AsSelectList(this IEnumerable<Factor> list)
        {
            return from item in list
                   select new SelectListItem() { Value = item.Id, Text = item.Name };
        }

        public static IEnumerable<SelectListItem> AsSelectList(this IEnumerable<int> list)
        {
            return from item in list
                   select new SelectListItem() { Value = item.ToString(), Text = item.ToString() };
        }
    }
}