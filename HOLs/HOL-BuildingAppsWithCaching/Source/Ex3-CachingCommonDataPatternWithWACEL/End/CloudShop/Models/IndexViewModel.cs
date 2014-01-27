namespace CloudShop.Models
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<string> Products { get; set; }

        public long ElapsedTime { get; set; }

        public bool IsCacheEnabled { get; set; }

        public bool IsLocalCacheEnabled { get; set; }

        public string ObjectId { get; set; }

        public string InstanceId
        {
            get
            {
                return Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.CurrentRoleInstance.Id;
            }
        }
    }
}