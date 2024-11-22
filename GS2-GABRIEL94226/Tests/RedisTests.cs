using Moq;
using StackExchange.Redis;
using Newtonsoft.Json;
using Xunit;
using GS2_GABRIEL94226.Services;
using GS2_GABRIEL94226.Models;

namespace GS2_GABRIEL94226.Tests
{
    public class RedisTests
    {
        private readonly RedisServices _redisCacheService;
        private readonly Mock<IDatabase> _mockDatabase;

        public RedisTests()
        {
            Mock<IDatabase>_mockDatabase = new Mock<IDatabase>();
            _redisCacheService = new RedisServices();
        }

        [Fact]
        public void TestSetCache()
        {
            var consumo = new Consumo
            {
                Quantidade = 100.5,
                Local = "Apartamento 201",
                Data = DateTime.Now
            };

            _redisCacheService.SetCache("consumo_1", consumo);

            _mockDatabase.Verify(db => db.StringSet(It.IsAny<RedisKey>(), It.IsAny<string>(), It.IsAny<TimeSpan?>(), When.Always, CommandFlags.None), Times.Once);
        }

        [Fact]
        public void TestGetCache()
        {
            var consumo = new Consumo
            {
                Quantidade = 100.5,
                Local = "Apartamento 201",
                Data = DateTime.Now
            };

            var consumoJson = JsonConvert.SerializeObject(consumo);
            var result = _redisCacheService.GetCache<Consumo>("consumo_1");

            _mockDatabase.Setup(db => db.StringGet(It.IsAny<RedisKey>())).Returns(consumoJson);


            Assert.NotNull(result);
            Assert.Equal(consumo.Local, result.Local);
            Assert.Equal(consumo.Quantidade, result.Quantidade);
        }
    }
}