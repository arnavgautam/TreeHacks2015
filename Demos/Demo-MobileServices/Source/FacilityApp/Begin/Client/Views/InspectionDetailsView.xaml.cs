// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234
namespace MobileClient.Views
{
    using System;

    using FacilityApp.Core;

    using MobileClient.Common;
    using MobileClient.Services;
    using MobileClient.ViewModels;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for
    /// the currently selected item.
    /// </summary>
    public sealed partial class InspectionDetailsView
    {
        private const int MinimumWidthForSupportingTwoPanes = 768;
        
        private readonly NavigationHelper navigationHelper;

        public InspectionDetailsView()
        {
            this.InitializeComponent();

            this.DefaultViewModel = new InspectionDetailsViewModel();
            this.DataContext = this;

            this.MapImage.Source = new BitmapImage(new Uri(ConfigurationHub.ReadConfigurationValue("MapImageLarge")));

            // Setup the navigation helper
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelperLoadState;

            // Setup the logical page navigation components that allow
            // the page to only show one pane at a time.
            this.navigationHelper.GoBackCommand = new RelayCommand(this.GoBack, this.CanGoBack);

            // Start listening for Window size changes 
            // to change from showing two panes to showing a single pane
            Window.Current.SizeChanged += this.WindowSizeChanged;
            this.InvalidateVisualState();
        }

        public InspectionDetailsViewModel DefaultViewModel { get; set; }
 
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }
            
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        private static string DetermineVisualState()
        {
            if (!UsingLogicalPageNavigation())
                return "PrimaryView";

            // Update the back button's enabled state when the view state changes
            var logicalPageBack = UsingLogicalPageNavigation();

            return logicalPageBack ? "SinglePane_Detail" : "SinglePane";
        }

        /// <summary>
        /// Invoked to determine whether the page should act as one logical page or two.
        /// </summary>
        /// <returns>True if the window should show act as one logical page, false
        /// otherwise.</returns>
        private static bool UsingLogicalPageNavigation()
        {
            return Window.Current.Bounds.Width < MinimumWidthForSupportingTwoPanes;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelperLoadState(object sender, LoadStateEventArgs e)
        {
            await this.DefaultViewModel.InitPageAsync();

            var request = e.NavigationParameter as FacilityRequestViewModel;
            this.DefaultViewModel.CurrentJob = request ?? new FacilityRequestViewModel()
            {
                User = ConfigurationHub.ReadConfigurationValue("UserName") + " " + ConfigurationHub.ReadConfigurationValue("UserSurname"),
                Building = ConfigurationHub.ReadConfigurationValue("BuildingFRVM"),
                Room = ConfigurationHub.ReadConfigurationValue("RoomFRVM"),
                RoomType = RoomType.Auditorium,
                RequestedDate = DateTime.Now,
                BeforeImageUrl = ConfigurationHub.ReadConfigurationValue("CameraThumbnail"),
                AfterImageUrl = ConfigurationHub.ReadConfigurationValue("CameraThumbnail"),
                DocId = new Random((int)DateTime.Now.Ticks & 0x0000FFFF).Next(100000).ToString(),
                Street = ConfigurationHub.ReadConfigurationValue("StreetFRVM"),
                City = ConfigurationHub.ReadConfigurationValue("CityFRVM"),
                State = ConfigurationHub.ReadConfigurationValue("StateFRVM"),
                Zip = ConfigurationHub.ReadConfigurationValue("ZipFRVM")
            };
        }

        private async void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            this.progressRing.IsActive = true;
            var facilityService = new FacilityService();
            btnAccept.IsEnabled = false;
            var job = this.DefaultViewModel.CurrentJob.GetFacilityRequest();

            if (string.IsNullOrEmpty(job.Id))
            {
                job.Id = Guid.NewGuid().ToString();
                await facilityService.InsertRequestAsync(job);
            }
            else
            {
                await facilityService.UpdateRequestAsync(job);
            }

            btnAccept.IsEnabled = true;
            this.progressRing.IsActive = false;
            this.GoBack();
        }

        /// <summary>
        /// Invoked with the Window changes size
        /// </summary>
        /// <param name="sender">The current Window</param>
        /// <param name="e">Event data that describes the new size of the Window</param>
        private void WindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        private bool CanGoBack()
        {
            return UsingLogicalPageNavigation() || this.navigationHelper.CanGoBack();
        }

        private void GoBack()
        {
            if (UsingLogicalPageNavigation())
            {
                // When logical page navigation is in effect and there's a selected item that
                // item's details are currently displayed.  Clearing the selection will return to
                // the item list.  From the user's point of view this is a logical backward
                // navigation.
               // this.itemGridView.SelectedItem = null;
                this.DefaultViewModel.CurrentJob = null;
            }
            else
            {
                this.navigationHelper.GoBack();
            }
        }

        private void InvalidateVisualState()
        {
            var visualState = DetermineVisualState();
            VisualStateManager.GoToState(this, visualState, false);
            this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
        }
    }
}