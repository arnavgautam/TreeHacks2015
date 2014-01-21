using Microsoft.WindowsAzure.MobileServices;
using EventBuddy.DataModel;
using EventBuddy.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EventBuddy
{
    public sealed partial class EventsPage : EventBuddy.Common.LayoutAwarePage
    {
        private async Task SaveEvent(Event item)
        {
            //TODO: save the new event
            
            Events.Add(item);
        }
        
        private async Task LoadEvents()
        {
            //TODO: query for existing events           
            
        }

        #region
        private ObservableCollection<Event> _events = new ObservableCollection<Event>();
        public dynamic Events
        {
            get { return _events; }
            set
            {
                _events.Clear();
                foreach (var item in value)
                {
                    _events.Add(item);
                }
            }
        }

        public EventsPage()
        {
            this.InitializeComponent();
            User.Current.PropertyChanged += this.OnUserPropertyChanged;
            this.SetLoggedUser();
            eventEditor.DataContext = new EventEditorViewModel();
            itemGridView.ItemsSource = _events;
        }

        private void OnUserPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UserId")
            {
                this.SetLoggedUser();
            }
        }

        private void SetLoggedUser()
        {
            this.loggedUser.Text = LoginMessageHelper.GetLoginMessage();
        }

        private async void RegisterChannel()
        {
            var showRetryMessageDialog = false;
            var retryMessageDialog = new MessageDialog("");

            try
            {
                if (GetPrivateClient() != null && GetPrivateClient().CurrentUser != null)
                {
                    var ch = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                    var channelDTO = new Channel { Uri = ch.Uri };
                    var channelTable = privateClient.GetTable<Channel>();

                    var id = ApplicationData.Current.LocalSettings.Values["ChannelId"];
                    if (id != null)
                    {
                        var channels = await channelTable.ToEnumerableAsync();

                        if (!channels.Where(item => item.Id.Equals(id)).Any())
                        {
                            ApplicationData.Current.LocalSettings.Values["ChannelId"] = null;
                        }
                    }

                    if (ApplicationData.Current.LocalSettings.Values["ChannelId"] == null)
                    {
                        await channelTable.InsertAsync(channelDTO);
                        ApplicationData.Current.LocalSettings.Values["ChannelId"] = channelDTO.Id;
                    }
                    else
                    {
                        channelDTO.Id = (int)ApplicationData.Current.LocalSettings.Values["ChannelId"];
                        await channelTable.UpdateAsync(channelDTO);
                    }
                }
            }
            catch(Exception e)
            {
                retryMessageDialog.Title = "There was a problem while registering the channel";
                retryMessageDialog.Content = e.Message;

                retryMessageDialog.Commands.Add(
                    new UICommand("Continue")
                    );

                // Set the command that will be invoked by default
                retryMessageDialog.DefaultCommandIndex = 0;

                // Set the command to be invoked when escape is pressed
                retryMessageDialog.CancelCommandIndex = 0;

                // Show the message dialog
                showRetryMessageDialog = true;
            }

            if (showRetryMessageDialog)
            {
                await retryMessageDialog.ShowAsync();
            }
        }

        MobileServiceClient privateClient = null;

        private MobileServiceClient GetPrivateClient()
        {
            if (privateClient == null)
            {
                var field = typeof(App).GetRuntimeFields().SingleOrDefault(pi => pi.Name == "MobileService");
                if (field == null)
                {
                    return null;
                }
                privateClient = field.GetValue(null) as MobileServiceClient;
            }
            return privateClient;
        }

        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (IsUsingMobileAuth() && !(GetPrivateClient() != null && GetPrivateClient().CurrentUser != null))
            {
                ShowMessage();
                return;
            }
            var eventItem = ((Event)e.ClickedItem);
            this.Frame.Navigate(typeof(SessionsPage), eventItem);
        }

        private bool IsUsingMobileAuth()
        {
            return this.GetPrivateClient() != null;
        }

        private async void ShowMessage()
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("You must log in in order to view an Event");

            messageDialog.Commands.Add(
                new UICommand("Close")
                );
            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        private async void btnSaveEvent(object sender, EventBuddy.EventEditor.SaveEditorEventArgs args)
        {
            var item = eventEditor.DataContext as EventEditorViewModel;
            await SaveEvent(item.Event);
            UpdateVisibility();
            eventEditor.Hide();
        }

        private async void btnEditEvent(object sender, RoutedEventArgs e)
        {
            //Edit of an Event NOT Required for Demo flow
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await LoadEvents();
            itemGridView.ItemsSource = _events;
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            loadingEventsIndicator.Visibility = Visibility.Collapsed;

            if (Events != null && Events.Count == 0)
            {
                noRecordsView.Visibility = Visibility.Visible;
                itemGridView.Visibility = Visibility.Collapsed;
            }
            else
            {
                noRecordsView.Visibility = Visibility.Collapsed;
                itemGridView.Visibility = Visibility.Visible;
            }
        }

        private void btnAddEvent(object sender, RoutedEventArgs e)
        {
            eventEditor.DataContext = new EventEditorViewModel();
            eventEditor.Show();
        }

        private async void onLoggedIn(object sender, EventArgs e)
        {
            RegisterChannel();
            await LoadEvents();
            itemGridView.ItemsSource = _events;
            UpdateVisibility();
        }

        private void OnAddEventTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

            btnAddEvent(sender, e);
        }

        #endregion
    }
}
