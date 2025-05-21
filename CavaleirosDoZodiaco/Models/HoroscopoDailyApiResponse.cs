using System.Text.Json.Serialization;

namespace CavaleirosDoZodiaco.Models
{
    public class HoroscopoData
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("horoscope_data")]
        public string HoroscopeMessage { get; set; }
    }

    public class HoroscopoDailyApiResponse
    {
        [JsonPropertyName("data")]
        public HoroscopoData Data { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
