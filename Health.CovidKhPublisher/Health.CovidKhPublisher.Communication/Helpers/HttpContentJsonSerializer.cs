using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Health.CovidKhPublisher.Communication.Helpers
{
    public sealed class HttpContentJsonSerializer
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Include
        };

        public string SerializeObject(object payload)
        {
            return JsonConvert.SerializeObject(payload, Settings);
        }

        public TResult DeserializeJsonFromStream<TResult>(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(streamReader);
            var serializer = new JsonSerializer();
            return serializer.Deserialize<TResult>(jsonReader);
        }
    }
}
