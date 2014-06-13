namespace MobileClient.Common
{
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Windows.ApplicationModel;

    public static class ConfigurationHub
    {
        public static string ReadConfigurationValue(string key)
        {
            var configurationPath = Path.Combine(Package.Current.InstalledLocation.Path, "Assets/Configuration/Settings.xml");
            var configurationXml = XDocument.Load(configurationPath);

            return configurationXml.Descendants("add").First(x => x.Attribute("key").Value == key).Attribute("value").Value;
        }
    }
}
