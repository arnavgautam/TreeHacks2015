using Microsoft.WindowsAzure.MobileServices;
using EventBuddy.DataModel;
using EventBuddy.Helpers;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace EventBuddy
{
    public sealed partial class Login : UserControl
    {        
        public async Task LoginTwitter()
        {
            await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Twitter);
        }

        public async Task LoginFacebook()
        {
            await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Facebook);
        }

        #region 

        public static Profile Profile { get; set; }
       
        static Login()
        {
        }

        public Login()
        {
            this.InitializeComponent();

            UpdateVisibility();
        }

        public event EventHandler<EventArgs> LoggedIn;

        private void ucTapped(object sender, TappedRoutedEventArgs e)
        {
            loginPopup.IsOpen = !loginPopup.IsOpen;         
        }

        private async void OnLoginFacebook(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                await LoginFacebook();
            }
            catch (Exception)
            {
                return;
            }

            var success = false;
            var shouldSleep = false;
            for (var iterator = 0; iterator < 3 && !success; iterator++)
            {
                try
                {
                    var facebookId = this.GetPrivateClient().CurrentUser.UserId.Split(':')[1];
                    var userInformation = await FacebookHelper.RetrieveUserInformation(facebookId);
                    User.Current.UserId = userInformation.FullName;
                    success = true;
                }
                catch (Exception)
                {
                    if (iterator == 2)
                    {
                        User.Current.UserId = GetPrivateClient().CurrentUser.UserId;
                    }
                    else
                    {
                        shouldSleep = true;
                    }
                }

                if (shouldSleep)
                {
                    shouldSleep = false;
                    await Task.Delay(TimeSpan.FromMilliseconds(400 * Math.Pow(2, iterator)));
                }
            }

            var login = LoggedIn;
            if (login != null)
                login.Invoke(this, null);

            UpdateVisibility();

            loginPopup.IsOpen = false;
        }

        private async void OnLoginTwitter(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                await LoginTwitter();
            }
            catch (Exception)
            {
                return;
            }

            var success = false;
            var shouldSleep = false;
            for (var iterator = 0; iterator < 3 && !success; iterator++)
            {
                try
                {
                    var twitterId = this.GetPrivateClient().CurrentUser.UserId.Split(':')[1];
                    var userInformation = await TwitterHelper.RetrieveUserInformation(twitterId);
                    User.Current.UserId = userInformation.Handle;
                    success = true;
                }
                catch (Exception)
                {
                    if (iterator == 2)
                    {
                        User.Current.UserId = GetPrivateClient().CurrentUser.UserId;
                    }
                    else 
                    {
                        shouldSleep = true;
                    }
                }

                if (shouldSleep)
                {
                    shouldSleep = false;
                    await Task.Delay(TimeSpan.FromMilliseconds(400 * Math.Pow(2, iterator)));                    
                }
            }

            var login = LoggedIn;
            if (login != null)
                login.Invoke(this, null);

            UpdateVisibility();

            loginPopup.IsOpen = false;
        }

        private void UpdateVisibility()
        {
            if (GetPrivateClient() != null && GetPrivateClient().CurrentUser != null)
                this.Visibility = Visibility.Collapsed;
            else
                this.Visibility = Visibility.Visible;
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
        #endregion
    }
}
