namespace MobileClient.ViewModels
{
    using System.ComponentModel;
    using System.IO;
    using System.Threading.Tasks;
    using FacilityApp.Core;
    using MobileClient.Common;
    using Windows.System.UserProfile;
    using Windows.UI.Xaml.Media.Imaging;

    public class ViewModelBase : INotifyPropertyChanged
    {
        private BitmapImage userImage;
        private string userName;
        private string userSurname;

        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapImage UserImage
        {
            get
            {
                return this.userImage;
            }

            set
            {
                this.userImage = value;
                this.NotifyPropertyChanged("UserImage");
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }

            set
            {
                this.userName = value;
                this.NotifyPropertyChanged("UserName");
            }
        }

        public string UserSurname
        {
            get
            {
                return this.userSurname;
            }

            set
            {
                this.userSurname = value;
                this.NotifyPropertyChanged("UserSurname");
            }
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected async Task GetUserImage()
        {
            var user = ConfigurationHub.ReadConfigurationValue("SharePointUser");
            var sharePointUser = user.Replace(".", "_").Replace("@", "_");

            var imageStream = await MobileClient.Util.SharePointProvider.GetUserPhoto(ConfigurationHub.ReadConfigurationValue("SharePointResource"), State.SharePointToken, sharePointUser);

            if (imageStream != null)
            {
                var memStream = new MemoryStream();
                var bitmap = new BitmapImage();

                await imageStream.CopyToAsync(memStream);
                memStream.Position = 0;
                bitmap.SetSource(memStream.AsRandomAccessStream());

                this.UserImage = bitmap;
            }
            else
            {
                this.UserImage = new BitmapImage(new System.Uri("ms-appx:///Assets/UserImage.png"));
            }
        }

        protected async Task InitUserInfo()
        {
            this.UserName = ConfigurationHub.ReadConfigurationValue("UserName");
            this.UserSurname = ConfigurationHub.ReadConfigurationValue("UserSurname");
            await this.GetUserImage();
        }
    }
}
