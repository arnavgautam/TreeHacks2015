namespace FacilityApp.UI.IOS.Util
{
    using System;
    using System.Diagnostics;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    public class ImageConnectionDelegate : NSUrlConnectionDelegate
    {
        private const int IncidentImageTag = 100;
        private readonly UITableView tableView;
        private readonly NSIndexPath index;
        private NSMutableData tempData;

        public ImageConnectionDelegate(UITableView tableView, NSIndexPath rowIndex)
        {
            this.tableView = tableView;
            this.index = rowIndex;
        }

        public override void FailedWithError(NSUrlConnection connection, NSError error)
        {
           // base.FailedWithError(connection, error);
        }
        
        public override void ReceivedData(NSUrlConnection connection, NSData data)
        {
            if (this.tempData == null)
            {
                this.tempData = new NSMutableData();
            }
            
            this.tempData.AppendData(data);
        }
        
        public override void FinishedLoading(NSUrlConnection connection)
        {
            var downloadedImage = UIImage.LoadFromData(this.tempData);
            this.tempData = null;
            this.InvokeOnMainThread(() =>
            {
                var imageView = this.tableView.CellAt(this.index).ViewWithTag(IncidentImageTag) as UIImageView;
                
                // check if the row was deallocated when the user scrolled away. ignore.
                if (imageView != null)
                {
                    imageView.Image = downloadedImage;
                }
            });
        }
    }
}