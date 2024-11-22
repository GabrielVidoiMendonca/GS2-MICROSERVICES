using MongoDB.Driver;
using GS2_GABRIEL94226.Models;

namespace GS2_GABRIEL94226.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<Consumo> _consumos;

        public MongoService()
        {
        
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("GS2-GABRIEL94226");
            _consumos = database.GetCollection<Consumo>("Consumos");
        }

        public async Task<Consumo> SalvarConsumoAsync(Consumo consumo)
        {
            await _consumos.InsertOneAsync(consumo);
            return consumo;
        }

        public async Task<List<Consumo>> RecuperarConsumosAsync()
        {
            return await _consumos.Find(consumo => true).ToListAsync();
        }
    }
}