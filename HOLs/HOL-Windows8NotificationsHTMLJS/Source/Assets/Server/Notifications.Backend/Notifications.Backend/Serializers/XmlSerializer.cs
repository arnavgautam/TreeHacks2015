namespace Notifications.Backend.Serializers
{
    using System.Xml;
    using Notifications.Backend.Infrastructure;

    public class XmlSerializer : IFormatSerializer
    {
        public string SerializeReply(object originalReply, out string contentType)
        {
            contentType = HttpConstants.MimeApplicationAtomXml;

            return (originalReply as XmlDocument).InnerXml;
        }
    }
}