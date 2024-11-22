using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using GS2_GABRIEL94226.Controllers;
using GS2_GABRIEL94226.Models;
using GS2_GABRIEL94226.Services;

namespace GS2_GABRIEL94226.Tests
{
    public class ConsumoTests
    {
        private readonly Mock<MongoService> _mockMongoService;
        private readonly Mock<RedisServices> _mockRedisCacheService;
        private readonly ConsumoController _controller;

        public ConsumoTests()
        {
            _mockMongoService = new Mock<MongoService>();
            _mockRedisCacheService = new Mock<RedisServices>();
            _controller = new ConsumoController(_mockMongoService.Object, _mockRedisCacheService.Object);
        }

        [Fact]
        public async Task TestRegistrarConsumo_Success()
        {
            var consumo = new Consumo
            {
                Quantidade = 120.5,
                Local = "Apartamento 201",
                Data = DateTime.Now
            };

            _mockMongoService.Setup(s => s.SalvarConsumoAsync(consumo)).ReturnsAsync(consumo);

            var result = await _controller.RegistrarConsumo(consumo);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }

        [Fact]
        public async Task TestRegistrarConsumo_BadRequest()
        {
            var consumo = new Consumo { Quantidade = -10, Local = "Apartamento 202" }; // Dados inválidos

            var result = await _controller.RegistrarConsumo(consumo);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task TestConsultarConsumos_Success()
        {
            var consumos = new List<Consumo>
            {
                new Consumo { Quantidade = 120.5, Local = "Apartamento 101", Data = DateTime.Now }
            };

            _mockMongoService.Setup(s => s.RecuperarConsumosAsync()).ReturnsAsync(consumos);

            var result = await _controller.ConsultarConsumos();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task TestConsultarConsumos_NotFound()
        {
            _mockMongoService.Setup(s => s.RecuperarConsumosAsync()).ReturnsAsync(new List<Consumo>());

            var result = await _controller.ConsultarConsumos();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}