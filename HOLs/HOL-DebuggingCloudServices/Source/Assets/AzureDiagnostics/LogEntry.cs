namespace AzureDiagnostics
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class LogEntry : TableEntity
    {
        public long EventTickCount { get; set; }

        public int Level { get; set; }

        public int EventId { get; set; }

        public int Pid { get; set; }

        public string Tid { get; set; }

        public string Message { get; set; }
    }
}
