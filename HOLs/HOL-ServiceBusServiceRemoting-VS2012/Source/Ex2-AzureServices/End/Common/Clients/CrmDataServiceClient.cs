// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Common.Clients
{
    using System;
    using System.ServiceModel;
    using Common.Contracts;

    public class CrmDataServiceClient : ICrmDataService
    {
        public Customer GetCustomer(Guid id)
        {
            var client = new CrmDataServiceChannel();
            Customer customer;
            
            try
            {
                customer = client.GetCustomer(id);
                client.Close();
            }
            catch (CommunicationException)
            {
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                client.Abort();
                throw;
            }
            catch (Exception)
            {
                client.Abort();
                throw;
            }

            return customer;
        }

        public Customer[] GetCustomers()
        {
            var client = new CrmDataServiceChannel();
            var customers = new Customer[] { };

            try
            {
                customers = client.GetCustomers();
                client.Close();
            }
            catch (CommunicationException)
            {
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                client.Abort();
                throw;
            }
            catch (Exception)
            {
                client.Abort();
                throw;
            }

            return customers;
        }

        public void UpdateCustomer(Customer customer)
        {
            var client = new CrmDataServiceChannel();

            try
            {
                client.UpdateCustomer(customer);
                client.Close();
            }
            catch (CommunicationException)
            {
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                client.Abort();
                throw;
            }
            catch (Exception)
            {
                client.Abort();
                throw;
            }
        }
    }
}