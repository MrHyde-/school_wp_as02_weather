using System.Runtime.Serialization;

namespace aWeatherApp.Model
{
    [DataContract]
    public class CityInForecast
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }
    }
}
