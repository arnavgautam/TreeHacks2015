namespace CustomerManager.StyleUI.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using CustomerManager.StyleUI.Common;
    using CustomerManager.StyleUI.DataModel;
    using WebApi.Models;

    public class GroupedCustomersViewModel : BindableBase
    {
        public ObservableCollection<CustomerViewModel> CustomersList { get; set; }

        public GroupedCustomersViewModel()
        {
            this.CustomersList = new ObservableCollection<CustomerViewModel>();

            this.GetCustomers();
        }

        private async void GetCustomers()
        { 
            IEnumerable<Customer> customers = await CustomersWebApiClient.GetCustomers();

            foreach (var customer in customers)
            {
                this.CustomersList.Add(new CustomerViewModel(customer));                
            }        
        }
    }
}
