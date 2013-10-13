using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace aWeatherApp.Model
{
    [DataContract]
    public class City
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "dt")]
        public string UnixTime { get; set; }

        [DataMember(Name = "main")]
        public Weather CurrentWeather { get; set; }

        [DataMember(Name = "sys")]
        public LocationData SystemData { get; set; }

        [DataMember(Name = "weather")]
        public List<WeatherExtraInfo> WeatherExtraInfos { get; set; }

        public string Location
        {
            get
            {
                string result = Name;

                if (SystemData != null)
                {
                    result += ", " + SystemData.Country;
                }

                return result;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("The city id is ");
            sb.Append(Id.ToString());
            sb.Append(", and the name is ");
            sb.Append(Location);

            sb.Append(".");
            return sb.ToString();
        }
    }
}
