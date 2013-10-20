using System.Runtime.Serialization;

namespace aWeatherApp.Model
{
    [DataContract]
    public class ForecastTemperatures
    {
        [DataMember(Name = "day")]
        public decimal TempDay { get; set; }
    }
}
