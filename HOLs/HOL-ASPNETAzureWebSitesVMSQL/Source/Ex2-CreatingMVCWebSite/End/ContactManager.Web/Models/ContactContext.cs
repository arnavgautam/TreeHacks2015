namespace ContactManager.Web.Models
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;    

    public class ContactContext : DbContext
    {
        public ContactContext()
            : base("ContactManagerDb")
        {
        }

        public DbSet<Contact> Contacts { get; set; }

        public IEnumerable<Contact> SearchContacts(string searchQuery)
        {            
            return this.Database.SqlQuery<Contact>("EXECUTE [dbo].[SearchContacts] @searchQuery", new SqlParameter("searchQuery", searchQuery ?? string.Empty));
        }
    }
}