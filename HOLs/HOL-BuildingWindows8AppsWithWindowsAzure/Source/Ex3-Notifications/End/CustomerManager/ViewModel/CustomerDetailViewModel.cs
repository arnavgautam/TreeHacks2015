namespace CustomerManager.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using CustomerManager.Common;
    using CustomerManager.Data;
    using WebApi.Models;

    public class CustomerDetailViewModel : BindableBase
    {
        private string selectedCustomerId;
        private CustomerViewModel selectedCustomer;

        public CustomerDetailViewModel()
        {
            this.CustomersList = new ObservableCollection<CustomerViewModel>();

            this.GetCustomers();
        }

        public string SelectedCustomerId
        {
            get
            {
                return this.selectedCustomerId;
            }

            set
            {
                this.SetProperty(ref this.selectedCustomerId, value);
                this.OnPropertyChanged("SelectedCustomer");
            }
        }

        public CustomerViewModel SelectedCustomer
        {
            get
            {
                return this.selectedCustomer;
            }

            set
            {
                this.SetProperty(ref this.selectedCustomer, value);
            }
        }

        public ObservableCollection<CustomerViewModel> CustomersList { get; set; }

        private async void GetCustomers()
        {
            IEnumerable<Customer> customers = await CustomersWebApiClient.GetCustomers();
            foreach (var customer in customers)
            {
                this.CustomersList.Add(new CustomerViewModel(customer));
            }

            this.SelectedCustomer = this.CustomersList.FirstOrDefault(c => c.CustomerId == this.SelectedCustomerId);
        }
    }
}
