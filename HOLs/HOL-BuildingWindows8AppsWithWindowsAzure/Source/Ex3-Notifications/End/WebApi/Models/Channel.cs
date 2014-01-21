namespace WebApi.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Channel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string Uri { get; set; }
    }
}