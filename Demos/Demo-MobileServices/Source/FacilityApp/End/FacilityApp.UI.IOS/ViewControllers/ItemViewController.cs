namespace FacilityApp.UI.IOS.ViewControllers
{
    using System;
    using FacilityApp.Core;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    [Register("ItemViewController")]
    public class ItemViewController : UIViewController
    {
        public ItemViewController(IntPtr handle) : base(handle)
        {
        }

        [Outlet]
        public UITextField UserNameInput { get; set; }

        [Outlet]
        public UITextField BuildingInput { get; set; }
        
        [Outlet]
        public UITextField RoomInput { get; set; }

        [Outlet]
        public UITextField DescriptionInput { get; set; }

        [Outlet]
        public UITextField DateInput { get; set; }

        [Outlet]
        public UITextField ServiceNotesInput { get; set; }

        [Outlet]
        public UITextField BeforeImage { get; set; }

        public FacilityRequest Request { get; set; }

        public override void ViewDidLoad()
        {
            if (this.Request != null)
            {
                this.UserNameInput.Text = this.Request.User;
                this.DateInput.Text = this.Request.RequestedDate.Date.ToShortDateString();
                this.DescriptionInput.Text = this.Request.ProblemDescription;
                this.BuildingInput.Text = this.Request.Building;
                this.RoomInput.Text = this.Request.Room;
                this.ServiceNotesInput.Text = this.Request.ServiceNotes;
            }
            else
            {
                this.NavigationController.NavigationBar.TopItem.Title = "Add a new Request";
                this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, this.Cancel);
                this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("Done", UIBarButtonItemStyle.Plain, this.AddNew);
            }
        }

        private void Cancel(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }

        // TODO: Implement
        private void AddNew(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }
    }
}