using System.ComponentModel;
namespace EventBuddy.WindowsPhone.Helpers
{
    public class User : INotifyPropertyChanged
    {
        private static User user;
        private string _userId;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private User()
        {
        }

        public string UserId
        {
            get
            {
                return _userId;
            }
            set 
            { 
                _userId = value;
                this.OnPropertyChanged("UserId");
            }
        }

        public static User Current
        {
            get
            {
                if (user == null)
                {
                    user = new User();
                }

                return user;
            }
        }
    }
}
