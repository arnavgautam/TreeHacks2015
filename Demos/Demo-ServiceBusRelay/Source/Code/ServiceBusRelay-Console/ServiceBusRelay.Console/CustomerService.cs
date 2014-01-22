namespace ServiceBusRelay.Console
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;

    [ServiceBehavior(Name = "CustomerService", Namespace = "http://samples.microsoft.com/ServiceModel/Relay/")]
    public class CustomerService : ICustomerContract
    {
        public IEnumerable<Customer> GetCustomers()
        {
            System.Console.WriteLine("Received GetCustomers() request.");

            var db = new NorthwindEntities();
            return db.Customers.Take(10).ToList();
        }

        public Customer GetCustomerById(string id)
        {
            System.Console.WriteLine("Received GetCustomerById() request.");

            var db = new NorthwindEntities();
            return db.Customers.Where(c => c.CustomerID == id).FirstOrDefault();
        }
    }
}
