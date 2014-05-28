namespace MobileClient
{
    using FacilityApp.Core;

    using MobileClient.Common;
    using MobileClient.ViewModels;
    using MobileClient.Views;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class MainPage
    {
        private readonly NavigationHelper navigationHelper;

        private readonly FacilityRequestListViewModel defaultViewModel = new FacilityRequestListViewModel();

        public MainPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.OnLoadState;   
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public FacilityRequestListViewModel DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public async void OnLoadState(object sender, LoadStateEventArgs e)
        {
            await this.DefaultViewModel.OnLoadState();
            prog1.IsActive = false;
        }
     
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void ItemViewItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(InspectionDetailsView), e.ClickedItem);
        }

        private void PageRootLoaded(object sender, RoutedEventArgs e)
        {
        }        

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(
                typeof(InspectionDetailsView),
                new FacilityRequest());
        }
    }
}
