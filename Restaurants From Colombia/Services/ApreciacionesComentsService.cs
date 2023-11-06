using MongoDB.Bson;
using MongoDB.Driver;
using Restaurants_From_Colombia.BD;
using Restaurants_From_Colombia.Model;
namespace Restaurants_From_Colombia.Services
{
    public class ApreciacionesComentsService
    {
        private readonly IMongoCollection<ApreciacionComentario> _apreciacionesComentsCollection;

        public ApreciacionesComentsService(MongoDBSettings mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.MongoDBConnection);
            var database = client.GetDatabase("restaurantManagement");           
            _apreciacionesComentsCollection = database.GetCollection<ApreciacionComentario>("apreciacionesComents");
           
        }

        public bool UsuarioHaDadoLikeAComentario(string username, string comentarioId)
        {
            var filter = Builders<ApreciacionComentario>.Filter.Eq("usuario", username) &
                         Builders<ApreciacionComentario>.Filter.Eq("comentario_id", ObjectId.Parse(comentarioId));

            var apreciacion = _apreciacionesComentsCollection.Find(filter).FirstOrDefault();
            return apreciacion != null;
        }

        public void RegistrarApreciacion(string username, string comentarioId)
        {
            var apreciacion = new ApreciacionComentario
            {
                Usuario = username,
                ComentarioId = ObjectId.Parse(comentarioId),
                Fecha = DateTime.UtcNow
            };

            _apreciacionesComentsCollection.InsertOne(apreciacion);
        }
    }

}
