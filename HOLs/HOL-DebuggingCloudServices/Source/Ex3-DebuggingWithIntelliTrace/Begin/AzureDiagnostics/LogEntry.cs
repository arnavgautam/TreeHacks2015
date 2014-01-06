namespace AzureDiagnostics
{
    using Microsoft.WindowsAzure.StorageClient;

    public class LogEntry : TableServiceEntity
    {
        public long EventTickCount { get; set; }

        public int Level { get; set; }

        public int EventId { get; set; }

        public int Pid { get; set; }

        public string Tid { get; set; }

        public string Message { get; set; }
    }
}
