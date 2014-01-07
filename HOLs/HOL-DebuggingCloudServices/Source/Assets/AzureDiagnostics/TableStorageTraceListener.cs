namespace AzureDiagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.Storage.Table.DataServices;

    public class TableStorageTraceListener : TraceListener
    {
        public static readonly string DiagnosticsTable = "DevLogsTable";

        private const string DefaultDiagnosticsConnectionString = "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString";

        [ThreadStatic]
        private static StringBuilder messageBuffer;

        private object initializationSection = new object();
        private bool isInitialized = false;

        private object traceLogAccess = new object();
        private List<LogEntry> traceLog = new List<LogEntry>();

        private CloudTableClient tableStorage;
        private string connectionString;

        public TableStorageTraceListener()
            : this(DefaultDiagnosticsConnectionString)
        {
        }

        public TableStorageTraceListener(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override bool IsThreadSafe
        {
            get
            {
                return true;
            }
        }

        public override void Write(string message)
        {
            if (TableStorageTraceListener.messageBuffer == null)
            {
                TableStorageTraceListener.messageBuffer = new StringBuilder();
            }

            TableStorageTraceListener.messageBuffer.Append(message);
        }

        public override void WriteLine(string message)
        {
            if (TableStorageTraceListener.messageBuffer == null)
            {
                TableStorageTraceListener.messageBuffer = new StringBuilder();
            }

            TableStorageTraceListener.messageBuffer.AppendLine(message);
        }

        public override void Flush()
        {
            if (!this.isInitialized)
            {
                lock (this.initializationSection)
                {
                    if (!this.isInitialized)
                    {
                        this.Initialize();
                    }
                }
            }

            var table = this.tableStorage.GetTableReference(DiagnosticsTable);

            TableServiceContext context = this.tableStorage.GetTableServiceContext();
            TableBatchOperation batchOperation = new TableBatchOperation();

            lock (this.traceLogAccess)
            {
                this.traceLog.ForEach(entry => batchOperation.Insert(entry));
                this.traceLog.Clear();
            }

            if (batchOperation.Count > 0)
            {
                table.ExecuteBatch(batchOperation);
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            base.TraceData(eventCache, source, eventType, id, data);
            this.AppendEntry(id, eventType, eventCache);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            base.TraceData(eventCache, source, eventType, id, data);
            this.AppendEntry(id, eventType, eventCache);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            base.TraceEvent(eventCache, source, eventType, id);
            this.AppendEntry(id, eventType, eventCache);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            base.TraceEvent(eventCache, source, eventType, id, format, args);
            this.AppendEntry(id, eventType, eventCache);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            base.TraceEvent(eventCache, source, eventType, id, message);
            this.AppendEntry(id, eventType, eventCache);
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            base.TraceTransfer(eventCache, source, id, message, relatedActivityId);
            this.AppendEntry(id, TraceEventType.Transfer, eventCache);
        }

        private void Initialize()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(this.connectionString));
            this.tableStorage = storageAccount.CreateCloudTableClient();
            CloudTable table = this.tableStorage.GetTableReference(DiagnosticsTable);
            table.CreateIfNotExists();

            this.isInitialized = true;
        }

        private void AppendEntry(int id, TraceEventType eventType, TraceEventCache eventCache)
        {
            if (TableStorageTraceListener.messageBuffer == null)
            {
                TableStorageTraceListener.messageBuffer = new StringBuilder();
            }

            string message = TableStorageTraceListener.messageBuffer.ToString();
            TableStorageTraceListener.messageBuffer.Length = 0;

            if (message.EndsWith(Environment.NewLine))
            {
                message = message.Substring(0, message.Length - Environment.NewLine.Length);
            }

            if (message.Length == 0)
            {
                return;
            }

            LogEntry entry = new LogEntry()
            {
                PartitionKey = string.Format("{0:D10}", eventCache.Timestamp >> 30),
                RowKey = string.Format("{0:D19}", eventCache.Timestamp),
                EventTickCount = eventCache.Timestamp,
                Level = (int)eventType,
                EventId = id,
                Pid = eventCache.ProcessId,
                Tid = eventCache.ThreadId,
                Message = message
            };

            lock (this.traceLogAccess)
            {
                this.traceLog.Add(entry);
            }
        }
    }
}
