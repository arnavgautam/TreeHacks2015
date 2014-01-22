using EventBuddy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventBuddy.DataModel
{
    public class Session : BindableBase
    {
        public Session()
        {
            Description = string.Empty;
            Room = string.Empty;
            Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 30, 0);
            End = Start.AddHours(1);
        }

        public Session (Event parent)
	    {
            EventId = parent.Id;

            Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 30, 0);
            End = Start.AddHours(1);
            Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum nostrud ipsum consectetur.";
            Room = "B33";
	    }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "eventId")]
        public int EventId { get; set; }

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

        private string _img;

        [DataMember(Name = "img")]
        public string Img 
        {
            get
            {
                if (_img == null)
                    return "Assets/NoProfile.png";
                return _img;
            }

            set
            {
                SetProperty(ref _img, value); 
            }
        }

        private string _deckSource;
        [DataMember(Name = "deckSource")]
        public string DeckSource
        {
            get 
            {
                if (_deckSource == null)
                    return string.Empty;
                return _deckSource; 
            }
            set 
            { 

                SetProperty(ref _deckSource, value); 
            }
        }

        private string _name;
        [DataMember(Name = "name")]
        public string Name
        {
            get
            {
                if (_name == null)
                    return string.Empty;
                return _name;
            }
            set
            {

                SetProperty(ref _name, value);
            }
        }
    }
}
