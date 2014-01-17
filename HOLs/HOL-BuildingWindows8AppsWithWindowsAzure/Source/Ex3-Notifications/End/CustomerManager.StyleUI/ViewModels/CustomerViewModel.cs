namespace CustomerManager.StyleUI.ViewModels
{
    using System;
    using CustomerManager.StyleUI.Common;
    using WebApi.Models;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;

    public class CustomerViewModel : BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        private int _customerId = 0;
        public int CustomerId
        {
            get { return this._customerId; }
            set { this.SetProperty(ref this._customerId, value); }
        }

        private string _name = string.Empty;
        public string Name
        {
            get { return this._name; }
            set { this.SetProperty(ref this._name, value); }
        }

        private string _company = string.Empty;
        public string Company
        {
            get { return this._company; }
            set { this.SetProperty(ref this._company, value); }
        }

        public string _phone = string.Empty;
        public string Phone
        {
            get { return this._phone; }
            set { this.SetProperty(ref this._phone, value); }
        }

        public string _email = string.Empty;
        public string Email
        {
            get { return this._email; }
            set { this.SetProperty(ref this._email, value); }
        }

        public string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }


        public string _address = string.Empty;
        public string Address
        {
            get { return this._address; }
            set { this.SetProperty(ref this._address, value); }
        }

        private String _imagePath = null;
        public String ImagePath
        {
            get
            {
                return this._imagePath;
            }
            set
            {
                this._image = null;
                this._imagePath = value;
                this.OnPropertyChanged("Image");
            }
        }

        private ImageSource _image = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(_baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public CustomerViewModel()
        {
        }

        public CustomerViewModel(Customer customer)
        {
            this.CustomerId = customer.CustomerId;
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
