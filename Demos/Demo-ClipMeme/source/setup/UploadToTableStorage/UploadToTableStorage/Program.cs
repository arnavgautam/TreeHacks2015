using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Configuration;


namespace UploadToTableStorage
{
    class Program
    {
        private static CloudStorageAccount storageAccount;
        
        static void Main(string[] args)
        {
            var accountName = args[0];
            var accountKey = args[1];
            var GifsDir = args[2];

            storageAccount =  CloudStorageAccount.Parse(String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",accountName,accountKey));
            
            // Create the blob client.
            CloudBlobClient blobClient = new CloudBlobClient (storageAccount.BlobEndpoint, storageAccount.Credentials);

            // Create blob container if it doesn't exist.
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("memeblob");
            blobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

            // Create the table client.
            CloudTableClient tableClient = new Microsoft.WindowsAzure.Storage.Table.CloudTableClient(storageAccount.TableEndpoint, storageAccount.Credentials);

            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference("MemeMetadata");
            var statusTable = table.CreateIfNotExists();

            var list = new List<ClipMemeEntity> {
                new ClipMemeEntity { BlobUri = "https://clipmeme2014.blob.core.windows.net/memes/20140401-200312-horse-cannon.gif", BlobName = "20140401-200312-horse-cannon.gif", Description = "Deploy", Username = "Mads"  },
                new ClipMemeEntity { BlobUri = "https://clipmeme2014.blob.core.windows.net/memes/20140401-200344-updated-visual-studio-theme.gif", BlobName = "20140401-200344-updated-visual-studio-theme.gif", Description = "News vs Theme", Username = "Mads"  },
                new ClipMemeEntity { BlobUri = "https://clipmeme2014.blob.core.windows.net/memes/20140401-200447-oops.gif", BlobName = "20140401-200447-oops.gif", Description = "First Iteration", Username = "Mads"  },
                new ClipMemeEntity { BlobUri = "https://clipmeme2014.blob.core.windows.net/memes/20140401-200534-just-learned-about-git-rebase.gif", BlobName = "20140401-200534-just-learned-about-git-rebase.gif", Description = "Tests Pass on First Try", Username = "Mads"  },
                new ClipMemeEntity { BlobUri = "https://clipmeme2014.blob.core.windows.net/memes/20140401-200606-when-i-unstash-something.gif", BlobName = "20140401-200606-when-i-unstash-something.gif", Description = "Scale up", Username = "Mads"  },
                new ClipMemeEntity { BlobUri = "https://clipmeme2014.blob.core.windows.net/memes/20140401-200750-dog-race.gif", BlobName = "20140401-200750-dog-race.gif", Description = "Sprint", Username = "Mads"  },
                new ClipMemeEntity { BlobUri = "https://clipmeme2014.blob.core.windows.net/memes/20140401-200800-cookies.gif", BlobName = "20140401-200800-cookies.gif", Description = "Scottgu Promoted", Username = "Mads"  },
                new ClipMemeEntity { BlobUri = "https://clipmeme2014.blob.core.windows.net/memes/20140401-200849-when-i-merge-my-own-pull-requests.gif", BlobName = "20140401-200849-when-i-merge-my-own-pull-requests.gif", Description = "to the cloud", Username = "Mads"  },
                new ClipMemeEntity { BlobUri = "https://clipmeme2014.blob.core.windows.net/memes/20140401-200924-disco-girl.gif", BlobName = "20140401-200924-disco-girl.gif", Description = "Hanseldance", Username = "Mads"  },
                new ClipMemeEntity { BlobUri = "https://clipmeme2014.blob.core.windows.net/memes/20140401-201030-when-someone-merges-your-pull-request-before-its-ready.gif", BlobName = "20140401-201030-when-someone-merges-your-pull-request-before-its-ready.gif", Description = "accidental git push", Username = "Mads"  },
                new ClipMemeEntity { BlobUri = "https://clipmeme2014.blob.core.windows.net/memes/20140401-201102-Fat-Dance-Suit-Men-Rave-On-At-Home.gif", BlobName = "20140401-201102-Fat-Dance-Suit-Men-Rave-On-At-Home.gif", Description = "msft at $40", Username = "Mads"  }                
            };

            foreach (var item in list)
	        {
                // Retrieve reference to a blob
                CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(item.BlobName);

                using (var fileStream = System.IO.File.OpenRead(String.Format("{0}\\{1}",GifsDir,item.BlobName)))
                {
                    blockBlob.UploadFromStream(fileStream);
                } 

                var requestOptions = new Microsoft.WindowsAzure.Storage.Table.TableRequestOptions()
                {
                    RetryPolicy = new Microsoft.WindowsAzure.Storage.RetryPolicies.LinearRetry(TimeSpan.FromMilliseconds(500), 5)
                };

                table.Execute(Microsoft.WindowsAzure.Storage.Table.TableOperation.Insert(item), requestOptions);
	        }

            return;
        }
    }

    public class ClipMemeEntity : TableEntity
    {
        public ClipMemeEntity()
        {
            this.PartitionKey = DateTime.UtcNow.ToString("yyyy-MM-dd");
            this.RowKey = Guid.NewGuid().ToString();
        }

        public string BlobName { get; set; }

        public string BlobUri { get; set; }

        public string Description { get; set;}

        public string Username { get; set; }
    }
}
