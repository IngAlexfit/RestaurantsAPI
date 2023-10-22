namespace Restaurants_From_Colombia.Services
{
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Restaurants_From_Colombia.BD;
    using Restaurants_From_Colombia.Model;

    public class RestauranteService
    {
        private readonly IMongoCollection<Restaurante> _restaurantesCollection;
        private readonly IMongoCollection<Comentario> _comentariosCollection;

        public RestauranteService(MongoDBSettings mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.MongoDBConnection);
            var database = client.GetDatabase("restaurantManagement");
            _restaurantesCollection = database.GetCollection<Restaurante>("restaurantes"); 
            _comentariosCollection = database.GetCollection<Comentario>("comentarios");

        }

        public List<Restaurante> GetRestaurantes()
        {
            // Realiza la consulta en la base de datos y devuelve la lista de restaurantes
            return _restaurantesCollection.Find(r => true).ToList();
        }
        public List<Comentario> GetComentariosPorRestaurante(int restauranteId)
        {
            var filter = Builders<Comentario>.Filter.Eq("restaurante_id", restauranteId);
            var comentarios = _comentariosCollection.Find(filter).ToList();
            return comentarios;
        }
        public List<Restaurante> GetRestaurantesPorCiudad(string ciudad)
        {
            return _restaurantesCollection.Find(r => r.Ubicacion == ciudad).ToList();
        }
        public Restaurante GetRestaurantePorId(int restauranteId)
        {
            var restaurante = _restaurantesCollection.Find(r => r.IdRestaur == restauranteId).SingleOrDefault();
            return restaurante;
        }
        public Comentario GetComentariosById(string comentarioId)
        {
            var comentario = _comentariosCollection.Find(c => c.Id == ObjectId.Parse(comentarioId)).SingleOrDefault();
            return comentario;
        }




        public void AgregarComentario(Comentario comentario)
        {
            _comentariosCollection.InsertOne(comentario);
        }

        public void IncrementarLike(string comentarioId)
        {
            var filter = Builders<Comentario>.Filter.Eq("_id", ObjectId.Parse(comentarioId));
            var update = Builders<Comentario>.Update.Inc("likes", 1); // Incrementa "likes" en 1
            _comentariosCollection.UpdateOne(filter, update);
        }

    }
    
    
}
