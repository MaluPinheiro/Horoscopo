using System.Text.Json.Serialization;

namespace CavaleirosDoZodiaco.Models
{
    public class HoroscopoSemanalData
    {
        [JsonPropertyName("week")]
        public string Week { get; set; }

        [JsonPropertyName("horoscope_data")]
        public string HoroscopeMessage { get; set; }
    }

    public class HoroscopoSemanalApiResponse
    {
        [JsonPropertyName("data")]
        public HoroscopoSemanalData Data { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
