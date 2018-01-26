using System.Runtime.Serialization;

namespace VAuto.Models
{
    [DataContract]
    public class Vehicle
    {
        [DataMember]
        public int vehicleId { get; set; }
        [DataMember]
        public int year { get; set; }
        [DataMember]
        public string make { get; set; }
        [DataMember]
        public string model { get; set; }
        [DataMember]
        public int dealerId { get; set; }
    }
}
