using System.Runtime.Serialization;

namespace aWeatherApp.Model
{
    [DataContract]
    public class ForecastTemperatures
    {
        [DataMember(Name = "day")]
        public decimal TempDay { get; set; }

        [DataMember(Name = "min")]
        public decimal TempMin { get; set; }

        [DataMember(Name = "max")]
        public decimal TempMax { get; set; }
    }
}
