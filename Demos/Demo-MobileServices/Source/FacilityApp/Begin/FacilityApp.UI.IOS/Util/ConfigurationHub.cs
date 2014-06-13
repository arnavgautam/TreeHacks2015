namespace FacilityApp.UI.IOS.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    public static class ConfigurationHub
    {
        public static string ReadConfigurationValue(string key)
        {
            var settingsDict = new NSDictionary(NSBundle.MainBundle.PathForResource("Settings.plist", null));
            if (settingsDict != null)
                return settingsDict[key].Description;
            return string.Empty;
        }
    }
}
