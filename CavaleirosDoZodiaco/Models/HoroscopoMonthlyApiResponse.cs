using System.Text.Json.Serialization;

namespace CavaleirosDoZodiaco.Models
{
    public class HoroscopoMensalData
    {
        [JsonPropertyName("month")]
        public string Month { get; set; }

        [JsonPropertyName("horoscope_data")]
        public string HoroscopeMessage { get; set; }

        [JsonPropertyName("challenging_days")]
        public string ChallengingDays { get; set; }

        [JsonPropertyName("standout_days")]
        public string StandoutDays { get; set; }
    }

    public class HoroscopoMensalApiResponse
    {
        [JsonPropertyName("data")]
        public HoroscopoMensalData Data { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
