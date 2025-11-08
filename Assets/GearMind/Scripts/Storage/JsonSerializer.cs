using Newtonsoft.Json;

namespace Assets.GearMind.Storage
{
    public class JsonSerializer : ISerializer<string>
    {
        private readonly JsonSerializerSettings _settings =
            new() { TypeNameHandling = TypeNameHandling.Auto };

        public string Serialize<T>(T data) => JsonConvert.SerializeObject(data, _settings);

        public T Deserialize<T>(string data) => JsonConvert.DeserializeObject<T>(data, _settings);
    }
}
