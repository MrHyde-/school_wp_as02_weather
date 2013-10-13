using System.Runtime.Serialization;

namespace aWeatherApp.Model
{
    [DataContract]
    public class WeatherExtraInfo
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "main")]
        public string Main { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "icon")]
        public string IconCode { get; set; }
    }
}
