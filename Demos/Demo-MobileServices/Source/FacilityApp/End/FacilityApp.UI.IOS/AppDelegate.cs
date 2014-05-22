namespace FacilityApp.UI.IOS
{
    using FacilityApp.Core;
    using FacilityApp.UI.IOS.Util;
    using Microsoft.WindowsAzure.MobileServices;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }

        public override void FinishedLaunching(UIApplication application)
        {
            CurrentPlatform.Init();

            UINavigationBar.Appearance.BarTintColor = new UIColor(red: 0, green: 173.0f / 255.0f, blue: 240.0f / 255.0f, alpha: 1);
            UINavigationBar.Appearance.BarTintColor.SetColor();

            UINavigationBar.Appearance.TintColor = UIColor.White;

            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() 
            {
                TextColor = UIColor.White
            });

            MobileServiceClientProvider.InitializeClient(ConfigurationHub.ReadConfigurationValue("MobSvcUri"), ConfigurationHub.ReadConfigurationValue("AppKey"));
        }
    }
}