using EventBuddy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventBuddy.DataModel
{
    public class Rating : BindableBase
    {
        private int _id;
        private int _sessionId;
        private float _score;

        [DataMember(Name="id")]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        
        [DataMember(Name = "sessionId")]
        public int SessionId
        {
            get { return _sessionId; }
            set { SetProperty(ref _sessionId, value); }
        }

        [DataMember(Name = "score")]
        public float Score
        {
            get { return _score; }
            set { SetProperty(ref _score, value); }
        }
        
        
    }
}
