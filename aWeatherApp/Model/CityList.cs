using System.Collections.Generic;
using System.Runtime.Serialization;

namespace aWeatherApp.Model
{
    [DataContract]
    public class CityList
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "cod")]
        public string Code { get; set; }

        [DataMember(Name = "count")]
        public long CityCount { get; set; }

        [DataMember(Name = "list")]
        public List<City> Cities { get; set; }
    }
}
