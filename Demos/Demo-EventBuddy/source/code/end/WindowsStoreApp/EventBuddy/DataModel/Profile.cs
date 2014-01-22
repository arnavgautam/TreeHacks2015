using EventBuddy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBuddy.DataModel
{
    public class Profile : BindableBase
    {
        private string _firstName;
        private string _lastName;
        private string _profileUri;

        public string FirstName 
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }
        public  string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }
        public string ProfileUri
        {
            get { return _profileUri; }
            set { SetProperty(ref _profileUri, value); }
        }
    }
}
