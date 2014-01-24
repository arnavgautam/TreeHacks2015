using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;
using Microsoft.ServiceBus.Notifications;
using System.Configuration;

namespace WebApi.Controllers
{
    public class CustomersController : ApiController
    {
        private CustomerContext db = new CustomerContext();

        // GET api/Customers
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers;
        }

        // GET api/Customers/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> GetCustomer(int id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT api/Customers/5
        public async Task<IHttpActionResult> PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Customers
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            await this.SendNotification(customer);

            return Ok(customer);
        }

        // DELETE api/Customers/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> DeleteCustomer(int id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerId == id) > 0;
        }

        private async Task SendNotificationAsync(Customer customer)
        {
            var connectionString = ConfigurationManager.AppSettings["HubConnectionString"];
            var notificationHub = ConfigurationManager.AppSettings["HubName"];
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, notificationHub);

            var toast = "<toast>" +
                            "<visual>" +
                                "<binding template=\"ToastText02\">" +
                                    "<text id=\"1\">New customer added!</text>" +
                                    "<text id=\"2\">" + customer.Name + "</text>" +
                                "</binding>" +
                            "</visual>" +
                        "</toast>";

            await hub.SendWindowsNativeNotificationAsync(toast);
        }
    }
}