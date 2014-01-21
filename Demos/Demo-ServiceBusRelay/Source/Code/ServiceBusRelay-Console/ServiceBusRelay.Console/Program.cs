namespace ServiceBusRelay.Console
{
    using System.ServiceModel.Web;
    using Microsoft.ServiceBus;
    using System;
    using System.ServiceModel;
    using System.Linq;
    using System.Configuration;
    using Microsoft.WindowsAzure;

    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            NamespaceManager namespaceClient = NamespaceManager.CreateFromConnectionString(connectionString);
            Uri address = new Uri(namespaceClient.Address, "Customer");
            ServiceHost host = new ServiceHost(typeof(CustomerService), address);

            TransportClientEndpointBehavior sharedSecretServiceBusCredential = new TransportClientEndpointBehavior();
            sharedSecretServiceBusCredential.TokenProvider = namespaceClient.Settings.TokenProvider;

            var endpoint = host.AddServiceEndpoint(typeof(ICustomerContract), new NetTcpRelayBinding("default"), address);
            endpoint.Behaviors.Add(sharedSecretServiceBusCredential);
            
            host.Open();

            checkDatabase();

            Console.WriteLine("Service address: " + address);
            Console.WriteLine("Press [Enter] to exit");
            Console.ReadLine();

            host.Close();
        }

        private static void checkDatabase()
        {
            try
            {
                var db = new NorthwindEntities();
                var read = db.Customers.Where(c => c.CustomerID == "!").FirstOrDefault();
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Connected to Database");
                Console.ForegroundColor = color;
            }
            catch (Exception exp)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exp.Message);
                Console.ForegroundColor = color;
            }
        }
    }
}
