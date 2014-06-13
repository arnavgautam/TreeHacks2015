namespace FacilityApp.UI.IOS.ViewControllers
{
    using System;
    using System.Collections.Generic;
    using FacilityApp.Core;
    using FacilityApp.UI.IOS.Services;
    using FacilityApp.UI.IOS.Util;
    using MBProgressHUD;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;
    
    [Register("RequestsTableViewController")]
    public class RequestsTableViewController : UITableViewController
    {
        private const string NavigateToDetailsSegueIdentifier = "navigateToDetails";

        private const string CellIdentifier = "RequestItemCell";
        
        private const int IncidentImageTag = 100;
        private const int TitleLabelTag = 101;
        private const int BuildingNumberLabelTag = 102;
        private const int OfficeNumberLabelTag = 103;
        private const int ReportingUserLabelTag = 104;
        private const int StatusLabelTag = 106;
        private const int StatusImageTag = 107;
        private const int AddressLabelTag = 108;

        private readonly FacilityService facilityService = new FacilityService();
        private readonly List<FacilityRequest> requests = new List<FacilityRequest>();
        private string mosconiCenterAddress = ConfigurationHub.ReadConfigurationValue("UserLocation");

        private FacilityRequest selectedRequest;
        private MTMBProgressHUD progressHud;
      
        public RequestsTableViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var refreshControl = new UIRefreshControl();
            this.RefreshControl = refreshControl;
            this.RefreshControl.ValueChanged += this.OnRefreshControlValueChanged;
            this.progressHud = new MTMBProgressHUD(View)
            {
                LabelText = "Loading...",
                RemoveFromSuperViewOnHide = true
            };

            View.AddSubview(this.progressHud);
            this.progressHud.Show(animated: true);

            this.facilityService.LoginCompletedAction = this.RetrieveRequests;
            this.facilityService.LoginAsync(true, ConfigurationHub.ReadConfigurationValue("AadAuthority"), ConfigurationHub.ReadConfigurationValue("AppRedirectLocation"), ConfigurationHub.ReadConfigurationValue("AadRedirectResourceURI"), ConfigurationHub.ReadConfigurationValue("AadClientID"));
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == NavigateToDetailsSegueIdentifier)
            {
                var itemViewController = (ItemViewController)segue.DestinationViewController;
                itemViewController.Request = this.selectedRequest;
            }
        }

        #region TableView Delegate and DataSource

        public override int NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return this.requests.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellIdentifier);
            var requestItem = this.requests[indexPath.Row];

            if (!string.IsNullOrEmpty(requestItem.BeforeImageUrl) && !requestItem.BeforeImageUrl.StartsWith("ms-appx"))
            {
                var imageRequest = new NSUrlRequest(new NSUrl(requestItem.BeforeImageUrl));
                var connection = new NSUrlConnection(imageRequest, new ImageConnectionDelegate(tableView, indexPath), true);
            }

            var titleLabel = (UILabel)cell.ViewWithTag(TitleLabelTag);
            titleLabel.Text = requestItem.ProblemDescription;

            var buildingLabel = (UILabel)cell.ViewWithTag(BuildingNumberLabelTag);
            buildingLabel.Text = requestItem.Building;

            var officeLabel = (UILabel)cell.ViewWithTag(OfficeNumberLabelTag);
            officeLabel.Text = requestItem.Room;

            var reportingUserLabel = (UILabel)cell.ViewWithTag(ReportingUserLabelTag);
            reportingUserLabel.Text = requestItem.User;

            var statusImage = (UIImageView)cell.ViewWithTag(StatusImageTag);
            var statusLabel = (UILabel)cell.ViewWithTag(StatusLabelTag);

            if (requestItem.CompletedDate > requestItem.RequestedDate)
            {
                statusLabel.Text = "Complete";
                statusLabel.TextColor = new UIColor(red: 0.0f / 255.0f, green: 173.0f / 255.0f, blue: 239.0f / 255.0f, alpha: 1.0f);
                statusImage.Image = UIImage.FromFile("Images/complete.png");
            }
            else
            {
                statusLabel.Text = "Incomplete";
                statusLabel.TextColor = new UIColor(red: 44.0f / 255.0f, green: 62.0f / 255.0f, blue: 80.0f / 255.0f, alpha: 1.0f);
                statusImage.Image = UIImage.FromFile("Images/incomplete.png");
            }

            var addressLabel = (UILabel)cell.ViewWithTag(AddressLabelTag);
            addressLabel.Text = this.mosconiCenterAddress;

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            
        }
        #endregion 

        private void OnRefreshControlValueChanged(object sender, EventArgs e)
        {
            this.InvokeOnMainThread(async () =>
            {
                var results = await this.facilityService.GetRequestsAsync();

                this.requests.Clear();
                this.requests.AddRange(results);
                this.TableView.ReloadData();
                this.RefreshControl.EndRefreshing();
            });
        }

        private void RetrieveRequests()
        {
            this.InvokeOnMainThread(async () =>
            {
                var results = await this.facilityService.GetRequestsAsync();

                this.requests.AddRange(results);
                this.TableView.ReloadData();
                this.progressHud.Hide(animated: true, delay: 1);
            });
        }
    }
}