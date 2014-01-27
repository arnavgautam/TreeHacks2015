namespace CustomerManager.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using CustomerManager.Common;
    using CustomerManager.DataModel;
    using WebApi.Models;

    public class GroupedCustomersViewModel : BindableBase
    {
        public GroupedCustomersViewModel()
        {
            this.CustomersList = new ObservableCollection<CustomerViewModel>();

            this.GetCustomers();
        }

        public ObservableCollection<CustomerViewModel> CustomersList { get; set; }

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
