using Newtonsoft.Json;

namespace Health.CovidKhPublisher.Communication.Models
{
    public class RnboMainDataApiResponse
    {
        [JsonProperty("dates")]
        public string[] Dates { get; set; }

        [JsonProperty("confirmed")]
        public int[] Confirmed { get; set; }

        [JsonProperty("deaths")]
        public int[] Deadths { get; set; }

        [JsonProperty("existing")]
        public int[] Existing { get; set; }

        [JsonProperty("recovered")]
        public int[] Recovered { get; set; }
    }
}
