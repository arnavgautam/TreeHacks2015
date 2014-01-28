using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectDemoApp
{
    class EFConnectionDemo
    {
        /// <summary>
        /// HolTestDbEntities takes care of handling your transactions for you
        /// leaving you free use Linq to extract information stores up in the cloud
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
