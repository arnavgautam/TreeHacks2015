using System.Data.Entity;

namespace WebApi.Models
{
    public class CustomerContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<WebApi.Models.CustomerContext>());

        public CustomerContext() : base("name=CustomerContext")
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }

    public class CustomerContextInitializer : DropCreateDatabaseIfModelChanges<CustomerContext>
    {
        protected override void Seed(CustomerContext context)
        {
            context.Customers.Add(new Customer
                {
                    Name = "Catherine Abel",
                    Email = "catherine.abel@vannuys.com",
                    Company = "Van Nuys",
                    Phone = "541 555 0100",
                    Address = "1 Microsoft Way, Redmond, WA, 98052",
                    Image = "Assets/CustomerPlaceholder.png",
                    Title = "Sales"
                });

            context.Customers.Add(new Customer
            {
                Name = "Kim Branch",
                Email = "kim.branch@contoso.com",
                Company = "Contoso",
                Phone = "541 555 0100",
                Address = "1 Microsoft Way, Redmond, WA, 98052",
                Image = "Assets/CustomerPlaceholder.png",
                Title = "Sales"
            });

            context.Customers.Add(new Customer
            {
                Name = "Frances Adams",
                Email = "frances.adams@contoso.com",
                Company = "Contoso",
                Phone = "541 555 0100",
                Address = " 1 Microsoft Way, Redmond, WA, 98052",
                Image = "Assets/CustomerPlaceholder.png",
                Title = "Sales"
            });

            context.Customers.Add(new Customer
            {
                Name = "Mark Harrington",
                Email = "mark.harrington@datum.com",
                Company = "A. Datum Corporation",
                Phone = "541 555 0100",
                Address = "1 Microsoft Way, Redmond, WA, 98052",
                Image = "Assets/CustomerPlaceholder.png",
                Title = "Sales"
            });

            context.Customers.Add(new Customer
            {
                Name = "Keith Harris",
                Email = "keith.harris@adventureworks.com",
                Company = "Adventure Works",
                Phone = "541 555 0100",
                Address = "1 Microsoft Way, Redmond, WA, 98052",
                Image = "Assets/CustomerPlaceholder.png",
                Title = "Sales"
            });

            context.Customers.Add(new Customer
            {
                Name = "Roger Harui",
                Email = "roger.harui@baldwinmuseum.com",
                Company = "Baldwin Museum of Art",
                Phone = "541 555 0100",
                Address = "1 Microsoft Way, Redmond, WA, 98052",
                Image = "Assets/CustomerPlaceholder.png",
                Title = "Sales"
            });

            context.Customers.Add(new Customer
            {
                Name = "Pilar Pinilla",
                Email = "pilar.pinilla@blueyonderairlines.com",
                Company = "Blue Yonder Airlines",
                Phone = "541 555 0100",
                Address = "1 Microsoft Way, Redmond, WA",
                Image = "Assets/CustomerPlaceholder.png",
                Title = "Sales"
            });

            context.Customers.Add(new Customer
            {
                Name = "Kari Hensien",
                Email = "kari.hensien@citypowerlight.com",
                Company = "City Power & Light",
                Phone = "541 555 0100",
                Address = "1 Microsoft Way, Redmond, WA",
                Image = "Assets/CustomerPlaceholder.png",
                Title = "Sales"
            });

            context.Customers.Add(new Customer
            {
                Name = "Johny Porter",
                Email = "johny.porter@cohowinery.com",
                Company = "Coho Winery",
                Phone = "541 555 0100",
                Address = "1 Microsoft Way, Redmond, WA",
                Image = "Assets/CustomerPlaceholder.png",
                Title = "Sales"
            });

            context.Customers.Add(new Customer
            {
                Name = "Peter Brehm",
                Email = "peter.brehm@cohowinery.com",
                Company = "Coho Winery",
                Phone = "541 555 0100",
                Address = "1 Microsoft Way, Redmond, WA",
                Image = "Assets/CustomerPlaceholder.png",
                Title = "Sales"
            });

            context.Customers.Add(new Customer
            {
                Name = "John Smith",
                Email = "john.smith@contoso.com",
                Company = "Contoso",
                Phone = "541 555 0100",
                Address = "1 Microsoft Way, Redmond, WA",
                Image = "Assets/CustomerPlaceholder.png",
                Title = "Sales"
            });

            context.SaveChanges();
        }
    }
}
