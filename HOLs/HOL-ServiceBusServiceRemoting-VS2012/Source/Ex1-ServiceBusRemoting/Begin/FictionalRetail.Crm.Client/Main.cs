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

namespace FictionalRetail.Crm.Client
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Common.Clients;
    using Common.Contracts;

    public partial class Main : Form
    {
        private PublicServiceClient client;
        
        public Main()
        {
            this.client = new PublicServiceClient();
            this.InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.LoadCustomersList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void MoveToBankingEntityButton_Click(object sender, EventArgs e)
        {
            var customersId = new List<Guid>();
            this.Cursor = Cursors.WaitCursor;

            try
            {
                foreach (ListViewItem item in this.customersListView.CheckedItems)
                {
                    var customerId = new Guid(item.SubItems[3].Text);
                    customersId.Add(customerId);
                }

                if (customersId.Count > 0)
                {
                    this.client.MoveCustomersToBankingEntity(customersId.ToArray(), BankingEntity.FictionalRetail);
                    this.LoadCustomersList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void LoadCustomersList()
        {
            try
            {
                Customer[] customers = this.client.ListCustomers();
                this.customersListView.Items.Clear();

                foreach (Customer customer in customers)
                {
                    var item = new ListViewItem(customer.Name);
                    item.SubItems.Add(customer.City);
                    item.SubItems.Add(customer.BankingEntity.ToString());
                    item.SubItems.Add(customer.Id.ToString());

                    this.customersListView.Items.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
