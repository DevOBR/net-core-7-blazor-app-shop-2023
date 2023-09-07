﻿using Newtonsoft.Json;

namespace AppShop.Share.Reponses
{
    public class StateResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("iso2")]
        public string? Iso2 { get; set; }
    }

}

