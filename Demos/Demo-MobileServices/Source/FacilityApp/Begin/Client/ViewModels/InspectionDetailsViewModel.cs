namespace MobileClient.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using FacilityApp.Core;
    using MobileClient.Common;

    public class InspectionDetailsViewModel : ViewModelBase    
    {
        private FacilityRequestViewModel currentJob = new FacilityRequestViewModel
            {
                User = ConfigurationHub.ReadConfigurationValue("UserName") + " " + ConfigurationHub.ReadConfigurationValue("UserSurname"),
                Building = ConfigurationHub.ReadConfigurationValue("BuildingFRVM"),
                Room = ConfigurationHub.ReadConfigurationValue("RoomFRVM"),
                RoomType = RoomType.Auditorium,
                RequestedDate = DateTime.Now,
                DocId = new Random((int)DateTime.Now.Ticks & 0x0000FFFF).Next(100000).ToString(),
                Street = ConfigurationHub.ReadConfigurationValue("StreetFRVM"),
                City = ConfigurationHub.ReadConfigurationValue("CityFRVM"),
                State = ConfigurationHub.ReadConfigurationValue("StateFRVM"),
                Zip = ConfigurationHub.ReadConfigurationValue("ZipFRVM")
            };

        public FacilityRequestViewModel CurrentJob
        {
            get
            {
                return this.currentJob;
            }

            set
            {
                this.currentJob = value;
                this.NotifyPropertyChanged("CurrentJob");
            }
        }

        public async Task InitPageAsync()
        {
            await this.InitUserInfo();
        }
    }
}
