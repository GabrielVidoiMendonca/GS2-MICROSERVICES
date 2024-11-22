using MongoDB.Driver;
using Mongo2Go;
using Xunit;
using GS2_GABRIEL94226.Services;
using GS2_GABRIEL94226.Models;


namespace GS2_GABRIEL94226.Tests
{
    public class MongoTests
    {
        private readonly MongoService _mongoService;
        private readonly MongoDbRunner _runner;

        public MongoTests()
        {
            
            _runner = MongoDbRunner.Start();
            var client = new MongoClient(_runner.ConnectionString);
            var database = client.GetDatabase("GS2-GABRIEL94226");
            _mongoService = new MongoService();
        }

        [Fact]
        public async Task TestSalvarConsumo()
        {
            var consumo = new Consumo
            {
                Quantidade = 120.5,
                Local = "Apartamento 101",
                Data = DateTime.Now
            };

            var consumoSalvo = await _mongoService.SalvarConsumoAsync(consumo);

            Assert.NotNull(consumoSalvo);
            Assert.Equal(consumo.Quantidade, consumoSalvo.Quantidade);
            Assert.Equal(consumo.Local, consumoSalvo.Local);
        }

        [Fact]
        public async Task TestRecuperarConsumos()
        {
            var consumo = new Consumo
            {
                Quantidade = 150.0,
                Local = "Apartamento 102",
                Data = DateTime.Now
            };

            await _mongoService.SalvarConsumoAsync(consumo);

            var consumos = await _mongoService.RecuperarConsumosAsync();

            Assert.NotEmpty(consumos);
            Assert.Equal(1, consumos.Count);
            Assert.Equal("Apartamento 102", consumos.First().Local);
        }
    }
}