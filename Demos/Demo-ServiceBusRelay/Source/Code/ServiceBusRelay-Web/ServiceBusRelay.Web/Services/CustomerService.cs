namespace ServiceBusRelay.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Threading;
    using Microsoft.ServiceBus;
    using Microsoft.WindowsAzure;
    using ServiceBusRelay.Console;

    public class CustomerService
    {
        static ChannelFactory<ICustomerChannel> channelFactory;
        static ICustomerChannel channel;

        public static IEnumerable<Customer> GetCustomers()
        {
            if (channelFactory == null)
            {
                GetChannelFactory();
            }

            int tries = 0;
            while (tries++ < 3)
            {
                try
                {
                    if (channel == null)
                    {
                        channel = channelFactory.CreateChannel();
                        channel.Open();
                    }

                    return channel.GetCustomers();
                }
                catch (CommunicationException)
                {
                    channel.Abort();
                    channel = null;

                    Thread.Sleep(500);
                }
            }

            return new List<Customer>();
        }

        public static Customer GetCustomerById(string id)
        {
            if (channelFactory == null)
            {
                GetChannelFactory();
            }

            int tries = 0;
            while (tries++ < 3)
            {
                try
                {
                    if (channel == null)
                    {
                        channel = channelFactory.CreateChannel();
                        channel.Open();
                    }

                    return channel.GetCustomerById(id);
                }
                catch (CommunicationException)
                {
                    channel.Abort();
                    channel = null;

                    Thread.Sleep(500);
                }
            }

            return new Customer();
        }

        private static void GetChannelFactory()
        {
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            NamespaceManager namespaceClient = NamespaceManager.CreateFromConnectionString(connectionString);
            Uri serviceUri = new Uri(namespaceClient.Address, "Customer");

            TransportClientEndpointBehavior sharedSecretServiceBusCredential = new TransportClientEndpointBehavior();
            sharedSecretServiceBusCredential.TokenProvider = namespaceClient.Settings.TokenProvider;

            channelFactory = new ChannelFactory<ICustomerChannel>("RelayEndpoint", new EndpointAddress(serviceUri));
            channelFactory.Endpoint.Behaviors.Add(sharedSecretServiceBusCredential);
        }
    }
}