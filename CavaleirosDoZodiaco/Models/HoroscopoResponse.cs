using System.Text.Json.Serialization;

namespace CavaleirosDoZodiaco.Models
{
    public class HoroscopoResponse
    {
        public string Signo { get; set; }
        public string Mensagem { get; set; }
        public string Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string NumeroDaSorte { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CorDoDia { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string BichoDoDia { get; set; }
    }
}
