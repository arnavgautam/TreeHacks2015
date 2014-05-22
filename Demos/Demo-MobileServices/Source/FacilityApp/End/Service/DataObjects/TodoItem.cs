namespace MobileService.DataObjects
{
    using Microsoft.WindowsAzure.Mobile.Service;

    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }
    }
}