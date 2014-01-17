namespace CustomerManager.StyleUI.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using CustomerManager.StyleUI.Common;
    using CustomerManager.StyleUI.DataModel;
    using WebApi.Models;

    public class CustomerDetailViewModel : BindableBase
    {
        private int _selectedCustomerId;
        public int SelectedCustomerId
        {
            get
            {
                return this._selectedCustomerId;
            }
            set
            {
                this.SetProperty(ref this._selectedCustomerId, value);
                this.OnPropertyChanged("SelectedCustomer");
            }
        }

        private CustomerViewModel _selectedCustomer;
        public CustomerViewModel SelectedCustomer
        {
            get
            {
                return this._selectedCustomer;
            }
            set
            {                
                this.SetProperty(ref this._selectedCustomer, value);                
            }

        }

        public ObservableCollection<CustomerViewModel> CustomersList { get; set; }

        public CustomerDetailViewModel()
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

            this.SelectedCustomer = this.CustomersList.FirstOrDefault(c => c.CustomerId == this.SelectedCustomerId);
        }
    }
}
