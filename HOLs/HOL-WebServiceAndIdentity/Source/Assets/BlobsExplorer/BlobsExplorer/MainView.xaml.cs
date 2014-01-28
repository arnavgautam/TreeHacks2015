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

namespace BlobsExplorer
{
    using System.Linq;
    using System.Windows;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;
    using System.IO;
    using System.Windows.Input;
    using System;
    using System.Diagnostics;

    public partial class MainView : Window
    {
        private CloudBlobClient client;

        public MainView()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            var account = new CloudStorageAccount(new StorageCredentialsAccountAndKey (AccountNameTextBox.Text, AccountKeyTextBox.Text), true);
            ConnectCloudBlobClient(account);
        }

        private void ConnectLocalButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectCloudBlobClient(CloudStorageAccount.DevelopmentStorageAccount);
        }

        private void ConnectCloudBlobClient(CloudStorageAccount account)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                client = CloudStorageAccountStorageClientExtensions.CreateCloudBlobClient(account);
                BlobContainerTextBox.IsEnabled = true;
                DownloadButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
            
                if (client == null)
                {
                    MessageBox.Show ("You are not connected to a Blob");
                    return;
                    
                }

                var container = this.client.GetContainerReference(BlobContainerTextBox.Text);

                if (!ContainerExists(container))
                {
                    MessageBox.Show (BlobContainerTextBox.Text + " container does not exist");
                    return;
                }

                System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
                folder.Description = "Provide the folder to save the Blobs";
                if (folder.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    MessageBox.Show("You must specify a folder");
                    return;
                }
                 

                var folderPath = folder.SelectedPath;
                
                var blobs = container.ListBlobs(new BlobRequestOptions
                {
                    UseFlatBlobListing = true,
                    BlobListingDetails = BlobListingDetails.Metadata
                });

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                foreach (CloudBlob blob in blobs)
                {
                    var fileName = blob.Uri.LocalPath;
                    fileName = fileName.Substring (fileName.IndexOf (container.Name) + container.Name.Length + 1);
                    fileName = fileName.Replace ("/", "_");
                    blob.DownloadToFile(Path.Combine(folderPath, fileName));
                }

                Process.Start("explorer.exe", folderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool ContainerExists(CloudBlobContainer container)
        {
            try
            {
                container.FetchAttributes();
            }
            catch (StorageClientException)
            {
                return false;
            }

            return true;
        }
    }
}