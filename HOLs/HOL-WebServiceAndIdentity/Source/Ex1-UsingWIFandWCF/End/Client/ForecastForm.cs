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
using Client.ServiceReference1;

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
            using (WeatherServiceClient relyingParty = new WeatherServiceClient())
            {
                WeatherInfo weatherInfo = null;

                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    this.sourceLabel.Text = "Loading...";

                    if (days == 3)
                    {
                        weatherInfo = relyingParty.GetThreeDaysForecast(zipCode);
                    }
                    else if (days == 10)
                    {
                        weatherInfo = relyingParty.GetTenDaysForecast(zipCode);
                    }

                    this.DisplayForecast(weatherInfo.Forecast);
                    this.sourceLabel.Text = string.Format(
                        CultureInfo.InvariantCulture,
                        "Source: {0}",
                        weatherInfo.Observatory);
                }
                catch (MessageSecurityException ex)
                {
                    this.sourceLabel.Text = string.Empty;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    relyingParty.Abort();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void DisplayForecast(Weather[] forecast)
        {
            this.forecastPanel.Controls.Clear();

            for (int i = 0; i < forecast.Length; i++)
            {
                PictureBox pic = new PictureBox();
                GroupBox box = new GroupBox();

                box.Text = string.Format(
                    CultureInfo.CurrentCulture,
                    "{0:ddd dd}: {1}",
                    DateTime.Today.AddDays(i),
                    forecast[i]);
                box.Height = 145;
                box.Width = 130;
                pic.Dock = DockStyle.Fill;
                pic.SizeMode = PictureBoxSizeMode.CenterImage;
                box.Controls.Add(pic);

                switch (forecast[i])
                {
                    case Weather.Sunny:
                        pic.Image = Resources.Sunny;
                        break;
                    case Weather.Cloudy:
                        pic.Image = Resources.Cloudy;
                        break;
                    case Weather.Snowy:
                        pic.Image = Resources.Snowy;
                        break;
                    case Weather.Rainy:
                        pic.Image = Resources.Rainy;
                        break;
                }

                this.forecastPanel.Controls.Add(box);
            }
        }     
    }
}