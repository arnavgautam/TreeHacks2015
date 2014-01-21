using EventBuddy.WindowsPhone.Dialogs;
using EventBuddy.WindowsPhone.Model;
using Microsoft.Phone.Controls;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace EventBuddy.WindowsPhone
{
    public partial class EventsPage : PhoneApplicationPage
    {
        static Popup popup = new Popup();
        bool showError = false;

        public EventsPage()
        {
            InitializeComponent();

            this.DataContext = this;

        }

        void EventsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.MobileService.CurrentUser == null && !App.MobileService.LoginInProgress && !showError)
            {
                this.ShowIdentityProviders();
            }
            else if (!showError)
            {
                LoadData();
            }
        }

        private async Task TwitterLogin()
        {
            try
            {
                await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Twitter);
                LoadData();
            }
            catch (Exception)
            {
                ShowConnectionError();
            }       
        }

        private void ShowConnectionError()
        {
            showError = true;
            popup = new Popup();
            popup.Height = Application.Current.Host.Content.ActualHeight;
            popup.Width = Application.Current.Host.Content.ActualWidth;
            popup.VerticalOffset = 30;
            ErrorDialog dialog = new ErrorDialog();
            dialog.Height = Application.Current.Host.Content.ActualHeight;
            dialog.Width = Application.Current.Host.Content.ActualWidth;
            popup.Child = dialog;
            popup.IsOpen = true;
            dialog.ErrorMessage.Text =
                "Oops! An error ocurred when connecting. " +
                "Make sure the configuration is correct and " +
                "retry again in a couple of seconds.";
            dialog.RetryButton.Click += (s, args) =>
            {
                popup.IsOpen = false;
                showError = false;
                ShowIdentityProviders();
            };

            dialog.CloseButton.Click += (s, args) =>
            {
                popup.IsOpen = false;
                Application.Current.Terminate();
            };
        }

        private async Task FacebookLogin()
        {
            try
            {
                await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Facebook);
                LoadData();
            }
            catch (Exception)
            {
                ShowConnectionError();
            }       
        }

        private async void LoadData()
        {
            this.LoadingProgress.IsIndeterminate = true;
            var events = await App.MobileService.GetTable<Event>().ToEnumerableAsync();
            Events.Clear();
            foreach (var item in events)
            {
                Events.Add(item);
            }

            this.LoadingProgress.IsIndeterminate = false;
        }

        private readonly ObservableCollection<Event> _events = new ObservableCollection<Event>();

        public ObservableCollection<Event> Events
        {
            get { return _events; }
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb != null)
            {
                Event selected = lb.SelectedItem as Event;
                if (selected != null)
                {
                    string uri = string.Format("/SessionsPage.xaml?id={0}&name={1}", selected.Id, selected.Name);

                    this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
                }
            }
        }

        private void ShowIdentityProviders()
        {
            popup.Height = Application.Current.Host.Content.ActualHeight;
            popup.Width = Application.Current.Host.Content.ActualWidth;
            popup.VerticalOffset = 30;
            IdentityDialog dialog = new IdentityDialog();
            dialog.Height = Application.Current.Host.Content.ActualHeight;
            dialog.Width = Application.Current.Host.Content.ActualWidth;
            popup.Child = dialog;
            popup.IsOpen = true;

            dialog.FacebookButton.Click += FacebookLogin_Click;
            dialog.TwitterButton.Click += TwitterLogin_Click;
        }

        private async void TwitterLogin_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
            await TwitterLogin();
        }

        private async void FacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
            await FacebookLogin();
        }

        private void RefreshAppBarButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoginAppBarButton_Click(object sender, EventArgs e)
        {
            this.ShowIdentityProviders();
        }
    }
}