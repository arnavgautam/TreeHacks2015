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

namespace FictionalInvestment.PublicServices
{
    using System;
    using System.Diagnostics;
    using Common.Clients;
    using Common.Contracts;

    public class CrmPublicService : ICrmPublicService
    {
        private CrmDataServiceClient client;
        
        public CrmPublicService()
        {
            this.client = new CrmDataServiceClient();
        }

        public void MoveCustomersToBankingEntity(Guid[] customerIds, BankingEntity bankingEntity)
        {
            foreach (Guid id in customerIds)
            {
                var customer = this.client.GetCustomer(id);
                customer.BankingEntity = bankingEntity;

                this.client.UpdateCustomer(customer);

                Console.ForegroundColor = ConsoleColor.Green;
                Trace.TraceInformation("[{0}] Customer {1} moved to {2}", DateTime.Now, customer.Name, bankingEntity);
                Console.ResetColor();
            }
        }

        public Customer[] ListCustomers()
        {
            return this.client.GetCustomers();
        }
    }
}