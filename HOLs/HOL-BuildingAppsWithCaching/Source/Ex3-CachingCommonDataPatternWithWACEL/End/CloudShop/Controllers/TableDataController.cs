using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloudShop.Models;
using System.Diagnostics;
using Microsoft.Ted.Wacel;
using Microsoft.WindowsAzure;
using Microsoft.ApplicationServer.Caching;

namespace CloudShop.Controllers
{
    public class TableDataController : ApiController
    {
        [HttpGet]
        public TableViewModel GetTable(string partition, string startId, string endId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Table<Customer> table = new Table<Customer>("Company", "Id", CloudConfigurationManager.GetSetting("StorageClient"), "customers", new DataCache("customers"));
            var customers = table.List(startId, endId, partition, partition).ToList();

            stopWatch.Stop();
            return new TableViewModel()
            {
                Customers = customers,
                ElapsedTime = stopWatch.ElapsedMilliseconds
            };
        }
    }
}
