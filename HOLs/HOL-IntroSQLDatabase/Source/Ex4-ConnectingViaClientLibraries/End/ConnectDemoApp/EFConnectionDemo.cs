using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectDemoApp
{
    public class EFConnectionDemo
    {
        /// <summary>
        /// HolTestDBEntities takes care of handling your transactions for you
        /// leaving you free use Linq to extract information stores up in th cloud
        /// </summary>
        public void ConnectToSQLDatabase()
        {
            using (HolTestDBEntities context = new HolTestDBEntities())
            {
                IQueryable<string> companyNames = from customer in context.Customers
                                                  where customer.CustomerID < 20
                                                  select customer.CompanyName;
                foreach (var company in companyNames)
                {
                    Console.WriteLine(company);
                }
            }
        }
    }
}
