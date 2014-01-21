namespace BuildClips.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Delegating message handler to profile calls to the Web API. The server execution time is returned as a 
    /// custom HTTP header inserted into the response. The handler also allows clearing the data cache on demand.
    /// </summary>
    /// <remarks>
    /// The measurement time includes the execution of all inner handlers in the pipeline. To exclude the execution 
    /// time of a handler from the total elapsed time, the handler needs to be added before the ApiExecutionProfiler  
    /// because handlers are called in the same order that they appear in the MessageHandlers collection. For example,  
    /// in the following scenario, Handler1 and Handler2 will not be included in the measurement.
    ///             config.MessageHandlers.Add(new Handler1());
    ///             config.MessageHandlers.Add(new Handler2());
    ///             config.MessageHandlers.Add(new ApiExecutionProfiler());
    ///             config.MessageHandlers.Add(new Handler3());
    ///             
    /// Clients set profiling options by including in their requests a custom HTTP header that specifies a comma separated 
    /// list of profiling options. For example:
    ///
    ///     Set-Profiling-Options: enable,clear-cache
    ///     
    /// Options include enabling execution measurements and removing all items from the data cache. For the latter, a 
    /// callback needs to be supplied in the constructor.
    /// </remarks>
    public class ApiExecutionProfiler : DelegatingHandler
    {
        // Custom HTTP Headers
        //  see http://tools.ietf.org/html/draft-ietf-appsawg-xdash-05 for naming considerations
        public const string ServerExecutionTimeHttpHeader = "Server-Execution-Time";
        public const string ProfilingOptionsHttpHeader = "Set-Profiling-Options";

        // Profiling options
        public const string EnableProfilingOption = "enable";       // specify this option to return execution time measurements
        public const string ClearDataCacheOption = "clear-cache";   // specify to remove all entries from the data cache

        private Action clearCache;

        public ApiExecutionProfiler()
            : this(null)
        {
        }

        public ApiExecutionProfiler(Action clearCache)
        {
            this.clearCache = clearCache;
        }

        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool profilingEnabled = ProcessProfilingOptions(request);

            var stopwatch = profilingEnabled ? Stopwatch.StartNew() : null;
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
           
            if (profilingEnabled)
            {
                stopwatch.Stop();
                response.Headers.Add(ApiExecutionProfiler.ServerExecutionTimeHttpHeader, stopwatch.ElapsedMilliseconds.ToString());
            }

            return response;
        }

        private bool ProcessProfilingOptions(HttpRequestMessage request)
        {
            bool profilingEnabled = false;

            IEnumerable<string> profilingOptions;
            if (request.Headers.TryGetValues(ApiExecutionProfiler.ProfilingOptionsHttpHeader, out profilingOptions))
            {
                foreach (var header in profilingOptions)
                {
                    foreach (var option in header.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (StringComparer.OrdinalIgnoreCase.Compare(ApiExecutionProfiler.EnableProfilingOption, option) == 0)
                        {
                            profilingEnabled = true;
                        }
                        else if (StringComparer.OrdinalIgnoreCase.Compare(ApiExecutionProfiler.ClearDataCacheOption, option) == 0)
                        {
                            if (this.clearCache != null)
                            {
                                this.clearCache();
                            }
                        }
                    }
                }
            }

            return profilingEnabled;
        }
    }
}
