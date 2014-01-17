namespace CustomerManager.StyleUI.ViewModels
{
    using CustomerManager.StyleUI.Common;
    using CustomerManager.StyleUI.DataModel;
    using WebApi.Models;

    public class NewCustomerViewModel : BindableBase
    {
        public CustomerViewModel Customer { get; set; }

        public NewCustomerViewModel()
        {
            this.Customer = new CustomerViewModel();
        }

        public void CreateCustomer()
        {
            CustomersWebApiClient.CreateCustomer(new Customer
            {
                CustomerId = this.Customer.CustomerId,
                Name = this.Customer.Name,
                Phone = this.Customer.Phone,
                Address = this.Customer.Address,                
                Email = this.Customer.Email,                
                Company = this.Customer.Company,
                Title = this.Customer.Title,
                Image = this.Customer.ImagePath,                
            });
        }
    }
}
