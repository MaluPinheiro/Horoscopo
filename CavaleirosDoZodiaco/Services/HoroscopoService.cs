using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CavaleirosDoZodiaco.Models;

namespace CavaleirosDoZodiaco.Services
{
    public interface IHoroscopoService
    {
        Task<HoroscopoResponse> GetDailyHoroscopo(string signo, string dia, string nickname = null);
        Task<HoroscopoResponse> GetWeeklyHoroscopo(string signo, string nickname = null);
        Task<HoroscopoResponse> GetMonthlyHoroscopo(string signo, string nickname = null);
        Usuario CadastrarUsuario(string nickname, string plano);

    }

    public class HoroscopoService : IHoroscopoService
    {
        private readonly Dictionary<string, string> _usuarios;
        private const string UsuariosPath = "Usuarios.json";

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://horoscope-app-api.vercel.app/api/v1/get-horoscope";
        private readonly Random _random;

        public HoroscopoService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _random = new Random();
            _usuarios = CarregarUsuariosDeArquivo();
        }

        private Dictionary<string, string> CarregarUsuariosDeArquivo()
        {
            if (!File.Exists(UsuariosPath))
                return new Dictionary<string, string>();

            var json = File.ReadAllText(UsuariosPath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                   ?? new Dictionary<string, string>();
        }
        private void SalvarUsuariosEmArquivo()
        {
            var json = JsonSerializer.Serialize(_usuarios, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(UsuariosPath, json);
        }
        public Usuario CadastrarUsuario(string nickname, string plano)
        {
            _usuarios[nickname.ToLower()] = plano.ToLower();
            SalvarUsuariosEmArquivo();

            return new Usuario
            {
                Nickname = nickname,
                Plano = plano
            };
        }

        public async Task<HoroscopoResponse> GetDailyHoroscopo(string signo, string dia, string nickname = null)
        {
            if (!IsValidDay(dia))
                throw new ArgumentException("Valor inválido para 'day'. Use: TODAY, TOMORROW, YESTERDAY ou uma data no formato YYYY-MM-DD");

            if (!IsValidZodiacSign(signo))
                throw new ArgumentException("Signo inválido. Use nomes em inglês como Aries, Taurus, etc.");

            // Padronização
            signo = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(signo.ToLower());
            dia = dia.ToUpper();

            var url = $"{ApiUrl}/daily?sign={signo}&day={dia}";

            var response = await _httpClient.GetFromJsonAsync<HoroscopoDailyApiResponse>(url);

            if (response == null || response.Data == null || string.IsNullOrWhiteSpace(response.Data.HoroscopeMessage))
                throw new Exception("Não foi possível obter os dados da API externa.");

            var result = new HoroscopoResponse
            {
                Signo = signo,
                Mensagem = response.Data.HoroscopeMessage,
                Data = response.Data.Date
            };

            if (!string.IsNullOrWhiteSpace(nickname) && _usuarios.TryGetValue(nickname.ToLower(), out var plano) && plano == "avancado")
            {
                result.NumeroDaSorte = GerarNumeroDaSorte();
                result.CorDoDia = EscolherCorDoDia();
                result.BichoDoDia = EscolherBichoDoDia();
            }

            return result;
        }

        public async Task<HoroscopoResponse> GetWeeklyHoroscopo(string signo, string nickname = null)
        {
            if (!IsValidZodiacSign(signo))
                throw new ArgumentException("Signo inválido.");

            signo = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(signo.ToLower());

            var url = $"{ApiUrl}/weekly?sign={signo}";
            var response = await _httpClient.GetFromJsonAsync<HoroscopoSemanalApiResponse>(url);

            if (response?.Data == null || string.IsNullOrWhiteSpace(response.Data.HoroscopeMessage))
                throw new Exception("Não foi possível obter o horóscopo semanal.");

            var result = new HoroscopoResponse
            {
                Signo = signo,
                Mensagem = response.Data.HoroscopeMessage,
                Data = response.Data.Week
            };

            if (!string.IsNullOrWhiteSpace(nickname) &&
                _usuarios.TryGetValue(nickname.ToLower(), out var plano) &&
                plano == "avancado")
            {
                result.NumeroDaSorte = GerarNumeroDaSorte();
                result.CorDoDia = EscolherCorDoDia();
                result.BichoDoDia = EscolherBichoDoDia();
            }

            return result;
        }

        public async Task<HoroscopoResponse> GetMonthlyHoroscopo(string signo, string nickname = null)
        {
            if (!IsValidZodiacSign(signo))
                throw new ArgumentException("Signo inválido.");

            signo = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(signo.ToLower());

            var url = $"{ApiUrl}/monthly?sign={signo}";
            var response = await _httpClient.GetFromJsonAsync<HoroscopoMensalApiResponse>(url);

            if (response?.Data == null || string.IsNullOrWhiteSpace(response.Data.HoroscopeMessage))
                throw new Exception("Não foi possível obter o horóscopo mensal.");

            var result = new HoroscopoResponse
            {
                Signo = signo,
                Mensagem = response.Data.HoroscopeMessage,
                Data = response.Data.Month + $" (dias bons: {response.Data.StandoutDays}, desafiadores: {response.Data.ChallengingDays})"
            };

            if (!string.IsNullOrWhiteSpace(nickname) &&
                _usuarios.TryGetValue(nickname.ToLower(), out var plano) &&
                plano == "avancado")
            {
                result.NumeroDaSorte = GerarNumeroDaSorte();
                result.CorDoDia = EscolherCorDoDia();
                result.BichoDoDia = EscolherBichoDoDia();
            }

            return result;
        }

        private bool IsValidDay(string dia)
        {
            if (string.IsNullOrWhiteSpace(dia))
                return false;

            dia = dia.ToUpper();
            if (dia == "TODAY" || dia == "TOMORROW" || dia == "YESTERDAY")
                return true;

            return DateTime.TryParse(dia, out _);
        }

        private bool IsValidZodiacSign(string signo)
        {
            var validSigns = new[] {
                "ARIES", "TAURUS", "GEMINI", "CANCER", "LEO", "VIRGO",
                "LIBRA", "SCORPIO", "SAGITTARIUS", "CAPRICORN", "AQUARIUS", "PISCES"
            };
            return validSigns.Contains(signo.ToUpper());
        }

        private static string GerarNumeroDaSorte() => new Random().Next(1, 100).ToString("D2");

        private static string EscolherCorDoDia()
        {
            var cores = new[] { "Azul", "Vermelho", "Amarelo", "Verde", "Roxo", "Laranja" };
            return cores[new Random().Next(cores.Length)];
        }

        private static string EscolherBichoDoDia()
        {
            var bichos = new[] { "Cavalo", "Gato", "Leão", "Cobra", "Águia", "Elefante" };
            return bichos[new Random().Next(bichos.Length)];
        }


    }
}
