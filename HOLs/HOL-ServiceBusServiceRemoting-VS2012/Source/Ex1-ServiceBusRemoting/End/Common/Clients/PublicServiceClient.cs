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

    public class PublicServiceClient : ICrmPublicService
    {
        public Customer[] ListCustomers()
        {
            var client = new PublicServiceChannel();
            var customers = new Customer[] { };

            try
            {
                customers = client.ListCustomers();
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

        public void MoveCustomersToBankingEntity(Guid[] customerIds, BankingEntity bankingEntity)
        {
            var client = new PublicServiceChannel();

            try
            {
                client.MoveCustomersToBankingEntity(customerIds, bankingEntity);
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
