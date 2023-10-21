namespace Restaurants_From_Colombia.Services
{
    using MongoDB.Driver;
    using Restaurants_From_Colombia.BD;
    using Restaurants_From_Colombia.Model;

    public class RestauranteService
    {
        private readonly IMongoCollection<Restaurante> _restaurantesCollection;

        public RestauranteService(MongoDBSettings mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.MongoDBConnection);
            var database = client.GetDatabase("restaurantManagement");
            _restaurantesCollection = database.GetCollection<Restaurante>("restaurantes");
        }

        public List<Restaurante> GetRestaurantes()
        {
            // Realiza la consulta en la base de datos y devuelve la lista de restaurantes
            return _restaurantesCollection.Find(r => true).ToList();
        }

        
    }
}
