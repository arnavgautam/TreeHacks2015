namespace MobileClient.ViewModels
{
    using System;
    using FacilityApp.Core;
    using MobileClient.Common;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;

    public class FacilityRequestViewModel : ViewModelBase
    {
        private string user;
        private string beforeImageUrl;
        private string afterImageUrl;
        private DateTimeOffset completedDate;

        private string status = "Incomplete";
        private string statusImage = ConfigurationHub.ReadConfigurationValue("StatusImageIncomplete");
        private Brush statusForegroundBrush = Application.Current.Resources["StatusIncompleteForegroundThemeBrush"] as SolidColorBrush;
        private string street;
        private string state;
        private string city;
        private string zip;

        public FacilityRequestViewModel()
        {
        }

        public FacilityRequestViewModel(FacilityRequest request)
        {
            this.Update(request);
        }

        public string Id { get; set; }

        public string User
        {
            get
            {
                return this.user;
            }

            set
            {
                this.user = value;
                this.NotifyPropertyChanged("User");
            }
        }

        public RoomType RoomType { get; set; }

        public string Building { get; set; }

        public string Room { get; set; }

        public string GeoLocation { get; set; }

        public string BTLEId { get; set; }

        public string BeforeImageUrl
        {
            get
            {
                return this.beforeImageUrl;
            }

            set
            {
                this.beforeImageUrl = value;
                this.NotifyPropertyChanged("BeforeImageUrl");
            }
        }

        public string AfterImageUrl
        {
            get { return this.afterImageUrl; }
            set
            {
                this.afterImageUrl = value;
                this.NotifyPropertyChanged("AfterImageUrl");
            }
        }

        public string ProblemDescription { get; set; }

        public string ServiceNotes { get; set; }

        public string DocId { get; set; }

        public DateTimeOffset RequestedDate { get; set; }

        public DateTimeOffset CompletedDate
        {
            get
            {
                return this.completedDate;
            }

            set
            {
                this.completedDate = value;
                if (this.completedDate > this.RequestedDate)
                {
                    this.Status = "Complete";
                    this.StatusImage = ConfigurationHub.ReadConfigurationValue("StatusImageComplete");
                    this.StatusForegroundBrush = Application.Current.Resources["StatusCompleteForegroundThemeBrush"] as SolidColorBrush;
                }
                else
                {
                    this.Status = "Incomplete";
                    this.StatusImage = ConfigurationHub.ReadConfigurationValue("StatusImageIncomplete");
                    this.StatusForegroundBrush = Application.Current.Resources["StatusIncompleteForegroundThemeBrush"] as SolidColorBrush;
                }

                this.NotifyPropertyChanged("CompletedDate");
            }
        }

        public string Version { get; set; }

        #region UX properties

        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
                this.NotifyPropertyChanged("Status");
            }
        }

        public string StatusImage
        {
            get { return this.statusImage; }

            set
            {
                this.statusImage = value;
                this.NotifyPropertyChanged("StatusImage");
            }
        }

        public Brush StatusForegroundBrush
        {
            get
            {
                return this.statusForegroundBrush;
            }

            set
            {
                this.statusForegroundBrush = value;
                this.NotifyPropertyChanged("StatusForegroundBrush");
            }
        }

        public string Street
        {
            get { return this.street; }

            set
            {
                this.street = value;
                this.NotifyPropertyChanged("Street");
            }
        }

        public string State
        {
            get { return this.state; }

            set
            {
                this.state = value;
                this.NotifyPropertyChanged("State");
            }
        }

        public string City
        {
            get { return this.city; }

            set
            {
                this.city = value;
                this.NotifyPropertyChanged("City");
            }
        }

        public string Zip
        {
            get { return this.zip; }

            set
            {
                this.zip = value;
                this.NotifyPropertyChanged("Zip");
            }
        }

        public void Update(FacilityRequest request)
        {
            this.Id = request.Id;
            this.User = request.User;
            this.RoomType = request.RoomType;
            this.Building = request.Building;
            this.Room = request.Room;
            this.GeoLocation = request.GeoLocation;
            this.Zip = request.Zip;
            this.Street = request.Street;
            this.State = request.State;
            this.City = request.City;
            this.BTLEId = request.BTLEId;
            this.BeforeImageUrl = request.BeforeImageUrl;
            this.AfterImageUrl = request.AfterImageUrl;
            this.ProblemDescription = request.ProblemDescription;
            this.ServiceNotes = request.ServiceNotes;
            this.DocId = request.DocId ?? new Random((int)DateTime.Now.Ticks & 0x0000FFFF).Next(100000).ToString();
            this.RequestedDate = request.RequestedDate;
            this.CompletedDate = request.CompletedDate;
            this.Version = request.Version;            
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
                Zip = this.Zip,
                Street = this.Street,
                State = this.State,
                City = this.City,
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

        #endregion
    }
}
