using System.Collections.Generic;
using System.Runtime.Serialization;

namespace aWeatherApp.Model
{
    [DataContract]
    public class ForecastList
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "cod")]
        public string Code { get; set; }

        [DataMember(Name = "city")]
        public CityInForecast City { get; set; }

        [DataMember(Name = "list")]
        public List<CityForecast> Forecasts { get; set; }
    }
}
