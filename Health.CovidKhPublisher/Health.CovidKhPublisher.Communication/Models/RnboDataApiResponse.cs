using Newtonsoft.Json;

namespace Health.CovidKhPublisher.Communication.Models
{
    public class RnboDataApiResponse
    {
        [JsonProperty("ukraine")]
        public Record[] UkraineRecords { get; set; }

        [JsonProperty("world")]
        public Record[] WorldRecords { get; set; }
    }

    public class Record
    {
        public int Id { get; set; }

        public Label Label { get; set; }

        public int Confirmed { get; set; }

        public int Deaths { get; set; }

        public int Recovered { get; set; }

        public int Existing { get; set; }

        public int Suspicion { get; set; }

        [JsonProperty("delta_confirmed")]
        public int DeltaConfirmed { get; set; }

        [JsonProperty("delta_deaths")]
        public int DeltaDeaths { get; set; }

        [JsonProperty("delta_recovered")]
        public int DeltaRecovered { get; set; }

        [JsonProperty("delta_existing")]
        public int DeltaExisting { get; set; }

        [JsonProperty("delta_suspicion")]
        public int DeltaSuspicion { get; set; }
    }

    public class Label
    {
        public string En { get; set; }
        public string Uk { get; set; }
    }
}
