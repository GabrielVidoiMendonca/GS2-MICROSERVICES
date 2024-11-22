using Microsoft.AspNetCore.Mvc;
using GS2_GABRIEL94226.Models;
using GS2_GABRIEL94226.Services;

namespace GS2_GABRIEL94226.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumoController : ControllerBase
    {

        private readonly MongoService _mongoService;
        private readonly RedisServices _redisCacheService;
        public ConsumoController(MongoService mongoService, RedisServices redisCacheService)
        {
            _mongoService = mongoService;
            _redisCacheService = redisCacheService;
        }
        private static List<Consumo> consumos = new List<Consumo>();

        [HttpPost("consumo")]
        public async Task<IActionResult> RegistrarConsumo([FromBody] Consumo consumo)
        {
            if (consumo == null)
            {
                return BadRequest("Dados inválidos.");
            }

            consumo.Id = consumos.Count + 1;
            consumo.Data = DateTime.Now;
            consumos.Add(consumo);

            {
                try
                {
                    var consumoSalvo = await _mongoService.SalvarConsumoAsync(consumo);
                    _redisCacheService.SetCache($"consumos", 5);
                    return CreatedAtAction(nameof(ConsultarConsumos), new { id = consumoSalvo.Id }, consumoSalvo);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Erro ao salvar consumo: {ex.Message}");
                }

            }
        }


        [HttpGet("consumo")]
        public async Task<IActionResult> ConsultarConsumos()
        {
            try
            {
                var consumos = await _mongoService.RecuperarConsumosAsync();
                var consumosCache = _redisCacheService.GetCache<List<Consumo>>("consumos");

                if (consumos.Count == 0)
                {
                    return NotFound("Nenhum dado encontrado.");
                }
                return Ok(consumos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao recuperar consumos: {ex.Message}");
            }
        }
    }
}