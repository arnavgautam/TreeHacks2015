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

namespace FictionalInvestment.Crm.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using Common.Contracts;

    public class CrmDataService : ICrmDataService
    {
        private readonly string xmlRepositoryPath;
        private List<Customer> customers;

        public CrmDataService()
        {
            var customersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Customers.xml");

            this.xmlRepositoryPath = customersFilePath;
            this.Load();
        }

        public Customer GetCustomer(Guid id)
        {
            return this.customers
                       .Where(c => c.Id.Equals(id))
                       .SingleOrDefault();
        }

        public Customer[] GetCustomers()
        {
            return this.customers
                       .OrderBy(c => c.Name)
                       .ToArray();
        }

        public void UpdateCustomer(Customer customer)
        {
            var originalCustomer = this.customers.Where(c => c.Id.Equals(customer.Id)).SingleOrDefault();

            originalCustomer.Name = customer.Name;
            originalCustomer.Address = customer.Address;
            originalCustomer.City = customer.City;
            originalCustomer.Zip = customer.Zip;
            originalCustomer.State = customer.State;
            originalCustomer.Country = customer.Country;
            originalCustomer.Email = customer.Email;
            originalCustomer.Phone = customer.Phone;
            originalCustomer.BankingEntity = customer.BankingEntity;

            this.Persist();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[{0}] Updated info for customer: {1}", DateTime.Now, customer.Name);
            Console.ResetColor();
        }

        private void Load()
        {
            try
            {
                using (var reader = new StreamReader(this.xmlRepositoryPath))
                {
                    var xmlSerializer = new XmlSerializer(typeof(Customer[]));
                    var deserializedCustomers = (Customer[])xmlSerializer.Deserialize(reader);
                    this.customers = new List<Customer>(deserializedCustomers);
                }
            }
            catch (FileNotFoundException)
            {
                this.customers = new List<Customer>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Persist()
        {
            using (var writer = new StreamWriter(this.xmlRepositoryPath))
            {
                var serial = new XmlSerializer(typeof(Customer[]));
                serial.Serialize(writer, this.customers.ToArray());
            }
        }
    }
}
