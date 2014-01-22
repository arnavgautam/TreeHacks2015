using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventBuddy.WindowsPhone.Model
{
    public class Rating
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "sessionId")]
        public int SessionId { get; set; }

        [DataMember(Name = "rating")]
        public float Score { get; set; }

        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "raterName")]
        public string RaterName { get; set; }
    }
}
