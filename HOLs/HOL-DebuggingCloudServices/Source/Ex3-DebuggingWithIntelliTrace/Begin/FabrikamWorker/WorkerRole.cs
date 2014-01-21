namespace FabrikamWorker
{
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("FabricamWorker entry point called", "Information");

            while (true)
            {
                Thread.Sleep(10000);

                if (!RoleEnvironment.IsEmulated)
                {
                    throw new CustomException("This is an example error.");
                }

                Trace.TraceInformation("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //// For information on handling configuration changes
            //// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
