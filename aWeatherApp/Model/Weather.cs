using System.Runtime.Serialization;

namespace aWeatherApp.Model
{
    [DataContract]
    public class Weather
    {
        [DataMember(Name = "temp")]
        public decimal Temperature { get; set; }

        [DataMember(Name = "pressure")]
        public decimal Pressure { get; set; }

        [DataMember(Name = "temp_min")]
        public decimal TempMin { get; set; }

        [DataMember(Name = "temp_max")]
        public decimal TempMax { get; set; }
    }
}
