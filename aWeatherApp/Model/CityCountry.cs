using System.Runtime.Serialization;

namespace aWeatherApp.Model
{
    [DataContract]
    public class CityCountry
    {
        [DataMember(Name = "country")]
        public string Country { get; set; }
    }
}