using MobileClient.Common;

namespace MobileClient.ViewModels
{
    using FacilityApp.Core;
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;

    public class FacilityRequestViewModel : ViewModelBase
    {
        public FacilityRequestViewModel ()
        {
               
        }
        public FacilityRequestViewModel(FacilityRequest request)
        {
            Update (request);
        }

        public void Update(FacilityRequest request)
        {
            Id = request.Id;
            User = request.User;
            RoomType = request.RoomType;
            Building = request.Building;
            Room = request.Room;
            GeoLocation = request.GeoLocation;
            BTLEId = request.BTLEId;
            BeforeImageUrl = request.BeforeImageUrl;
            AfterImageUrl = request.AfterImageUrl;
            ProblemDescription = request.ProblemDescription;
            ServiceNotes = request.ServiceNotes;
            DocId = request.DocId ?? new Random(((int)DateTime.Now.Ticks & 0x0000FFFF)).Next(100000).ToString();
            RequestedDate = request.RequestedDate;
            CompletedDate = request.CompletedDate;
            Version = request.Version;            
        }

        public FacilityRequest GetFacilityRequest() 
        {
            return new FacilityRequest {
                Id = this.Id,
                User = this.User,
                RoomType = this.RoomType,
                Building = this.Building,
                Room = this.Room,
                GeoLocation = this.GeoLocation,
                BTLEId = this.BTLEId,
                BeforeImageUrl = this.BeforeImageUrl ?? ConfigurationHub.ReadConfigurationValue("CameraThumbnail"),
                AfterImageUrl = this.AfterImageUrl ?? ConfigurationHub.ReadConfigurationValue("CameraThumbnail"),
                ProblemDescription = this.ProblemDescription,
                ServiceNotes = this.ServiceNotes,
                DocId = this.DocId,
                RequestedDate = this.RequestedDate,
                CompletedDate = this.CompletedDate,
                Version = this.Version,
            };
        }

        public string Id { get; set; }
        private string _user;

        public string User
        {
            get { return _user; }
            set { 
                _user = value;
                NotifyPropertyChanged("User");
            
            }
        }
        
        public RoomType RoomType { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }
        public string GeoLocation { get; set; }
        public string BTLEId { get; set; }
        private string _beforeImageUrl;

        public string BeforeImageUrl
        {
            get { 
                return _beforeImageUrl; 
            }
            set
            {
                _beforeImageUrl = value;
                NotifyPropertyChanged("BeforeImageUrl");
            }
        }

        private string _afterImageUrl;

        public string AfterImageUrl
        {
            get { return _afterImageUrl; }
            set
            {
                _afterImageUrl = value;
                NotifyPropertyChanged("AfterImageUrl");
            }
        }

        public string ProblemDescription { get; set; }
        public string ServiceNotes { get; set; }
        public string DocId { get; set; }
        public DateTimeOffset RequestedDate { get; set; }
        private DateTimeOffset _completedDate;
        public DateTimeOffset CompletedDate 
        { 
            get
            { 
                return _completedDate; 
            }
            set
            {
                _completedDate = value;
                if(_completedDate > RequestedDate)
                {
                    Status = "Complete";
                    StatusImage = ConfigurationHub.ReadConfigurationValue("StatusImageComplete");
                    StatusForegroundBrush = Application.Current.Resources["StatusCompleteForegroundThemeBrush"] as SolidColorBrush;
                }
                else
                {
                    Status = "Incomplete";
                    StatusImage = ConfigurationHub.ReadConfigurationValue("StatusImageIncomplete");
                    StatusForegroundBrush = Application.Current.Resources["StatusIncompleteForegroundThemeBrush"] as SolidColorBrush;
                }
               
                NotifyPropertyChanged("CompletedDate");
            }
        }

        public string Version { get; set; }

        #region UX properties
        private string _status = "Incomplete";
        public string Status
        {
            get 
            { 
                return _status; 
            }
            set 
            { 
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        private string _statusImage = ConfigurationHub.ReadConfigurationValue("StatusImageIncomplete");

        public string StatusImage
        {
            get { return _statusImage; }
            set 
            { 
                _statusImage = value;
                NotifyPropertyChanged("StatusImage");
            }
        }

        private Brush _statusForegroundBrush = Application.Current.Resources["StatusIncompleteForegroundThemeBrush"] as SolidColorBrush;

        public Brush StatusForegroundBrush
        {
            get
            {
                return _statusForegroundBrush;
            }
            set
            {
                _statusForegroundBrush = value;
                NotifyPropertyChanged("StatusForegroundBrush");
            }
        }

        private string _street;

        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                NotifyPropertyChanged("Street");
            }
        }

        private string _state;

        public string State
        {
            get { return _state; }
            set 
            { 
                _state = value;
                NotifyPropertyChanged("State");
            }
        }

        private string _city;

        public string City
        {
            get { return _city; }
            set 
            { 
                _city = value;
                NotifyPropertyChanged("City");
            }
        }

        private string _zip;

        public string Zip
        {
            get { return _zip; }
            set 
            { 
                _zip = value;
                NotifyPropertyChanged("Zip");
            }
        }
        

        #endregion

    }
}
