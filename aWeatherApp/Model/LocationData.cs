using System.Runtime.Serialization;

namespace aWeatherApp.Model
{
    [DataContract]
    public class LocationData
    {
        [DataMember(Name = "country")]
        public string Country { get; set; }
    }
}