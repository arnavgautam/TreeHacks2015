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

using System;
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Windows.Forms;
using Client.Properties;

namespace Client
{
    public partial class ForecastForm : Form
    {
        public ForecastForm()
        {
            this.InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback = ValidateCert;
        }

        private static bool ValidateCert(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            // If running against the staging environment, disable the client’s CN check
            // Danger Danger: Not Secure
            return true;
        }

       private void GetThreeDaysButton_Click(object sender, EventArgs e)
        {
            int zipCode = int.Parse(this.zipCodeTextBox.Text, CultureInfo.InvariantCulture);
            this.ShowForecast(3, zipCode);
        }

        private void GetTenDaysButton_Click(object sender, EventArgs e)
        {
            int zipCode = int.Parse(this.zipCodeTextBox.Text, CultureInfo.InvariantCulture);
            this.ShowForecast(10, zipCode);
        }

        private void ShowForecast(int days, int zipCode)
        {
        }
    }
}