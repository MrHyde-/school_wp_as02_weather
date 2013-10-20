using System.Collections.Generic;
using System.Runtime.Serialization;

namespace aWeatherApp.Model
{
    [DataContract]
    public class CityForecast
    {
        [DataMember(Name = "dt")]
        public string UnixTime { get; set; }

        [DataMember(Name = "temp")]
        public ForecastTemperatures Temperatures { get; set; }

        [DataMember(Name = "pressure")]
        public decimal Pressure { get; set; }

        [DataMember(Name = "humidy")]
        public decimal Humidy { get; set; }
        
        [DataMember(Name = "weather")]
        public List<WeatherExtraInfo> WeatherExtraInfos { get; set; }
    }
}
