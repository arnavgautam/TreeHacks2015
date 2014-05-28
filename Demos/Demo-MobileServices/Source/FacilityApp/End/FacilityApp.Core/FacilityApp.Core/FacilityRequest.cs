namespace FacilityApp.Core
{
    using System;
    using Newtonsoft.Json;
    
    public class FacilityRequest  
    {
        public string Id { get; set; }

        public string User { get; set; }

        public RoomType RoomType { get; set; }

        public string Building { get; set; }

        public string Room { get; set; }

        public string GeoLocation { get; set; }

        public string Zip { get; set; }

        public string Street { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string BTLEId { get; set; }

        public string BeforeImageUrl { get; set; }

        public string AfterImageUrl { get; set; }

        public string ProblemDescription { get; set; }

        public string ServiceNotes { get; set; }

        public string DocId { get; set; }

        public DateTimeOffset RequestedDate { get; set; }

        public DateTimeOffset CompletedDate { get; set; }

        [JsonProperty(PropertyName = "__version")]
        public string Version { get; set; }
    }
}
