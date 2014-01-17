namespace CustomerManager.StyleUI
{
    using System;
    using System.Collections.Generic;
    using CustomerManager.StyleUI.Common;
    using CustomerManager.StyleUI.DataModel;
    using CustomerManager.StyleUI.ViewModels;
    using Windows.UI.Xaml.Controls;

    public sealed partial class GroupedCustomersPage : LayoutAwarePage
    {
        private GroupedCustomersViewModel ViewModel { get; set; }

        public GroupedCustomersPage()
        {            
            this.InitializeComponent();
            this.ViewModel = new GroupedCustomersViewModel();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {            
            this.DataContext = this.ViewModel;
        }

        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void CustomerItem_Click(object sender, ItemClickEventArgs e)
        {
            var customerId = ((CustomerViewModel)e.ClickedItem).CustomerId;            
            
            this.Frame.Navigate(typeof(CustomerDetailPage), customerId);
        }

        private void NewCustomer_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewCustomerPage));
        }
    }
}
