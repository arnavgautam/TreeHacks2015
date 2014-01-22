using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventBuddy.WindowsPhone.Model
{
    public class Session
    {
        public Session()
        {
            Start = End = DateTime.Now;
        }

        public Session(Event parent)
        {
            EventId = parent.Id;
            Start = End = parent.Start;
        }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "eventId")]
        public int EventId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "speaker")]
        public string Speaker { get; set; }

        [DataMember(Name = "room")]
        public string Room { get; set; }

        [DataMember(Name = "start")]
        public DateTime Start { get; set; }

        [DataMember(Name = "end")]
        public DateTime End { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "img")]
        public string Img { get; set; }

        private string _deckSource = string.Empty;

        [DataMember(Name = "deckSource")]
        public string DeckSource
        {
            get 
            {
                return _deckSource; 
            }
            set 
            { 
                _deckSource = value; 
            }
        }
    }
}
