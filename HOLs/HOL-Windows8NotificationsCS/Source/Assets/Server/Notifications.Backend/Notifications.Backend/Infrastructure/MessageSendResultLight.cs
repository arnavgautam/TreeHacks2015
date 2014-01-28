namespace Notifications.Backend.Infrastructure
{
    public class MessageSendResultLight
    {
        public const string Success = "Success";
        public const string Error = "Error";

        public string Status { get; set; }

        public string Description { get; set; }
    }
}
