namespace MobileClient.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using FacilityApp.Core;
    using Services;
    using Windows.UI.Xaml;

    public class FacilityRequestListViewModel : ViewModelBase
    {
        private readonly FacilityService facilityService = new FacilityService();

        private ObservableCollection<FacilityRequestViewModel> requests;

        private Visibility gridVisibility;

        public ObservableCollection<FacilityRequestViewModel> Requests
        {
            get
            {
                return this.requests;
            }

            set
            {
                this.requests = value;
                this.NotifyPropertyChanged("Requests");
            }
        }

        public Visibility GridVisibility
        {
            get
            {
                return this.gridVisibility;
            }

            set
            {
                this.gridVisibility = value;
                this.NotifyPropertyChanged("GridVisibility");
            }
        }

        public async Task OnLoadState()
        {
            this.GridVisibility = Visibility.Collapsed;

            await this.AuthenticateAsync();
            await this.GetUserImage();
            await this.InitPageAsync();
        }

        public async Task RefreshRequests()
        {
            var items = await this.facilityService.GetRequestsAsync();
            this.Requests = new ObservableCollection<FacilityRequestViewModel>(from r in items
                                                                               select new FacilityRequestViewModel(r));
        }

        public async Task InitPageAsync()
        {
            await this.InitUserInfo();
            await this.RefreshRequests();
        }

        private async Task AuthenticateAsync()
        {
            while (MobileServiceClientProvider.MobileClient.CurrentUser == null)
            {
                var aadAuthority = ConfigurationHub.ReadConfigurationValue("AadAuthority");
                var aadRedirectResourceUri = ConfigurationHub.ReadConfigurationValue("AadRedirectResourceURI");
                var aadClientId = ConfigurationHub.ReadConfigurationValue("AadClientID");
                await this.LoginAdalAsync(aadAuthority, aadRedirectResourceUri, aadClientId);
            }

            if (MobileServiceClientProvider.MobileClient.CurrentUser != null)
            {
                this.GridVisibility = Visibility.Visible;
            }
        }

        private async Task LoginAdalAsync(string authority, string resourceUri, string clientId)
        {
            await this.facilityService.LoginAsync(false, authority, string.Empty, resourceUri, clientId);
        }
    }
}
