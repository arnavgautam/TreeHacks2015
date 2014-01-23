namespace CustomerManager.ViewModel
{
    using System;
    using CustomerManager.Common;
    using WebApi.Models;

    public class CustomerViewModel : BindableBase
    {
        private string customerId = string.Empty;
        private string name = string.Empty;
        private string company = string.Empty;
        private string phone = string.Empty;
        private string email = string.Empty;
        private string address = string.Empty;
        private string title = string.Empty;
        private string imagePath = string.Empty;

        public string CustomerId
        {
            get { return this.customerId; }
            set { this.SetProperty(ref this.customerId, value); }
        }

        public string Name
        {
            get { return this.name; }
            set { this.SetProperty(ref this.name, value); }
        }

        public string Company
        {
            get { return this.company; }
            set { this.SetProperty(ref this.company, value); }
        }

        public string Phone
        {
            get { return this.phone; }
            set { this.SetProperty(ref this.phone, value); }
        }

        public string Email
        {
            get { return this.email; }
            set { this.SetProperty(ref this.email, value); }
        }

        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        public string Address
        {
            get { return this.address; }
            set { this.SetProperty(ref this.address, value); }
        }

        public string ImagePath
        { 
            get { return this.imagePath; }
            set { this.SetProperty(ref this.imagePath, value); }
        }

        public CustomerViewModel()
        {
        }

        public CustomerViewModel(Customer customer)
        {
            this.CustomerId = customer.CustomerId.ToString();
            this.Name = customer.Name;
            this.Phone = customer.Phone;
            this.Address = customer.Address;
            this.Email = customer.Email;
            this.Company = customer.Company;
            this.Title = customer.Title;
            this.ImagePath = string.IsNullOrEmpty(customer.Image) ? "/Assets/CustomerPlaceholder.png" : customer.Image;
        }
    }
}
