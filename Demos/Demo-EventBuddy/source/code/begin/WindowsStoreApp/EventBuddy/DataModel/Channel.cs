using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventBuddy.DataModel
{
    public class Channel
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        [DataMember(Name="uri")]
        public string Uri { get; set; }
    }
}
