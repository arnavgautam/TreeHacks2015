using EventBuddy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBuddy.DataModel
{
    public class EventEditorViewModel:BindableBase
    {
        public EventEditorViewModel()
        {
            Editing = false;
            Event = new Event();
        }

        private bool _editing;
        public bool Editing
        {
            get { return _editing; }
            set
            {
                if (value != _editing)
                {                    
                    SetProperty(ref _editing, value);
                }
            }
        }

        private Event _event;
        public Event Event 
        {
            get { return _event; }
            set { SetProperty(ref _event, value); }
        }
    }
}
