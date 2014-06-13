// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app 
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app 
// is first launched.

namespace MobileClient.DataModel
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FacilityApp.Core;

    public sealed class SampleDataSource
    {
        public static async Task SynchronizeDataAsync()
        {
        }

        public static async Task UpdateJobAsync(FacilityRequest job)
        {
            await FacilityServiceBase.MobileClient.GetTable<FacilityRequest>().InsertAsync(job);
        }

        public static async Task<List<FacilityRequest>> GetJobsAsync()
        {
            return new List<FacilityRequest>
            {
                new FacilityRequest()
                {
                    Id = "11111", Building = "9", Room = "2260"
                }
            };
        }
    }
}