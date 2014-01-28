namespace Notifications.Backend.Serializers
{
    public interface IFormatSerializer
    {
        string SerializeReply(object originalReply, out string contentType);
    }
}
