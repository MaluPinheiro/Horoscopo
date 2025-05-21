using CavaleirosDoZodiaco.Models;
using CavaleirosDoZodiaco.Services;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

namespace CavaleirosDoZodiaco.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoroscopeController : ControllerBase
    {
        private readonly IHoroscopoService _horoscopeService;

        public HoroscopeController(IHoroscopoService horoscopeService)
        {
            _horoscopeService = horoscopeService;
        }

        [HttpGet("diario")]
        public async Task<IActionResult> GetDailyHoroscope(
            [FromQuery] string signo,
            [FromQuery] string dia = "TODAY",
            [FromQuery] string nickname = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(signo))
                    return BadRequest("O par�metro 'signo' � obrigat�rio");

                var horoscopo = await _horoscopeService.GetDailyHoroscopo(signo, dia, nickname);
                return Ok(horoscopo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
                return StatusCode(500, "Erro interno ao processar o hor�scopo.");
            }
        }

        [HttpGet("semanal")]
        public async Task<IActionResult> GetWeeklyHoroscope(
            [FromQuery] string signo,
            [FromQuery] string nickname = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(signo))
                    return BadRequest("O par�metro 'signo' � obrigat�rio");

                var horoscopo = await _horoscopeService.GetWeeklyHoroscopo(signo, nickname);
                return Ok(horoscopo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
                return StatusCode(500, "Erro interno ao processar o hor�scopo semanal.");
            }
        }

        [HttpGet("mensal")]
        public async Task<IActionResult> GetMonthlyHoroscope(
            [FromQuery] string signo,
            [FromQuery] string nickname = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(signo))
                    return BadRequest("O par�metro 'signo' � obrigat�rio");

                var horoscopo = await _horoscopeService.GetMonthlyHoroscopo(signo, nickname);
                return Ok(horoscopo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
                return StatusCode(500, "Erro interno ao processar o hor�scopo mensal.");
            }
        }

        [HttpPost("cadastro")]
        public IActionResult CadastrarUsuario([FromBody] Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nickname) || string.IsNullOrWhiteSpace(usuario.Plano))
                return BadRequest("Nickname e plano s�o obrigat�rios.");

            if (usuario.Plano.ToLower() != "basico" && usuario.Plano.ToLower() != "avancado")
                return BadRequest("Plano deve ser 'basico' ou 'avancado'.");

            _horoscopeService.CadastrarUsuario(usuario.Nickname, usuario.Plano);
            return Ok($"Usu�rio '{usuario.Nickname}' cadastrado com plano '{usuario.Plano}'.");
        }


    }
}