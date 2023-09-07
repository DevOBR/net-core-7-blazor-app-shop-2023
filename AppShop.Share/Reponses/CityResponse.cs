using Newtonsoft.Json;

namespace AppShop.Share.Reponses
{
    public class CityResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }

}

