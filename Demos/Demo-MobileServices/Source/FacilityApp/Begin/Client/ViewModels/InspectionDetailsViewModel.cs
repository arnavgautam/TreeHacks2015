using System;
using FacilityApp.Core;
using MobileClient.Common;

namespace MobileClient.ViewModels
{
    public class InspectionDetailsViewModel : ViewModelBase    
    {
        private FacilityRequestViewModel _currentJob = new FacilityRequestViewModel
            {
                User = ConfigurationHub.ReadConfigurationValue("UserName") + " " + ConfigurationHub.ReadConfigurationValue("UserSurname"),
                Building = ConfigurationHub.ReadConfigurationValue("BuildingFRVM"),
                Room = ConfigurationHub.ReadConfigurationValue("RoomFRVM"),
                RoomType = RoomType.Auditorium,
                RequestedDate = DateTime.Now,
                DocId = new Random(((int)DateTime.Now.Ticks & 0x0000FFFF)).Next(100000).ToString(),
                Street = ConfigurationHub.ReadConfigurationValue("StreetFRVM"),
                City = ConfigurationHub.ReadConfigurationValue("CityFRVM"),
                State = ConfigurationHub.ReadConfigurationValue("StateFRVM"),
                Zip = ConfigurationHub.ReadConfigurationValue("ZipFRVM")
            };
        public FacilityRequestViewModel CurrentJob
        {
            get
            {
                return _currentJob;
            }
            set
            {
                _currentJob = value;
                NotifyPropertyChanged("CurrentJob");
            }
        }
    }
}
