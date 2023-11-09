using MongoDB.Bson;
using MongoDB.Driver;
using Restaurants_From_Colombia.BD;
using Restaurants_From_Colombia.Model;
namespace Restaurants_From_Colombia.Services
{
    public class ComentsService
    {
        private readonly IMongoCollection<ApreciacionComentario> _apreciacionesComentsCollection;
        private readonly IMongoCollection<Comentario> _comentariosCollection;
        public ComentsService(MongoDBSettings mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.MongoDBConnection);
            var database = client.GetDatabase("restaurantManagement");           
            _apreciacionesComentsCollection = database.GetCollection<ApreciacionComentario>("apreciacionesComents");
            _comentariosCollection = database.GetCollection<Comentario>("comentarios");


        }
        public void AgregarComentario(Comentario comentario)
        {
            _comentariosCollection.InsertOne(comentario);
        }

        
        public void IncrementarLike(ObjectId comentarioId)
        {
            var filter = Builders<Comentario>.Filter.Eq("_id", comentarioId);
            var update = Builders<Comentario>.Update.Inc("likes", 1); 
            _comentariosCollection.UpdateOne(filter, update);
        }

        public void DecrementarLike(ObjectId comentarioId)
        {
            var filter = Builders<Comentario>.Filter.Eq("_id", comentarioId);
            var update = Builders<Comentario>.Update.Inc("likes", -1); // Decrementar en 1
            _comentariosCollection.UpdateOne(filter, update);
        }
        public void IncrementarDiskLike(ObjectId comentarioId)
        {
            var filter = Builders<Comentario>.Filter.Eq("_id", comentarioId);
            var update = Builders<Comentario>.Update.Inc("dislikes", 1); 
            _comentariosCollection.UpdateOne(filter, update);
        }
        public void DecrementarDiskLike(ObjectId comentarioId)
        {
            var filter = Builders<Comentario>.Filter.Eq("_id", comentarioId);
            var update = Builders<Comentario>.Update.Inc("dislikes", -1); // Decrementar en 1
            _comentariosCollection.UpdateOne(filter, update);
        }


        public bool UsuarioHaDadoLikeAComentario(string username, ObjectId comentarioId)
        {
            var filter = Builders<ApreciacionComentario>.Filter.Eq("usuario", username) &
                         Builders<ApreciacionComentario>.Filter.Eq("comentario_id", comentarioId)&
                          Builders<ApreciacionComentario>.Filter.Eq("accion", "like");

            var apreciacion = _apreciacionesComentsCollection.Find(filter).FirstOrDefault();
            return apreciacion != null;
        }

        public bool UsuarioHaDadoDiskLikeAComentario(string username, ObjectId comentarioId)
        {
            var filter = Builders<ApreciacionComentario>.Filter.Eq("usuario", username) &
                         Builders<ApreciacionComentario>.Filter.Eq("comentario_id", comentarioId) &
                          Builders<ApreciacionComentario>.Filter.Eq("accion", "dislike");

            var apreciacion = _apreciacionesComentsCollection.Find(filter).FirstOrDefault();
            return apreciacion != null;
        }


        public void RegistrarApreciacion(string username, ObjectId comentarioId, string accion)
        {
            var filter = Builders<ApreciacionComentario>.Filter.And(
                Builders<ApreciacionComentario>.Filter.Eq("Usuario", username),
                Builders<ApreciacionComentario>.Filter.Eq("ComentarioId", comentarioId)
            );

            var update = Builders<ApreciacionComentario>.Update.Set("accion", accion);

            var result = _apreciacionesComentsCollection.UpdateOne(filter, update);

            if (result.ModifiedCount == 0)
            {
                var apreciacion = new ApreciacionComentario
                {
                    Usuario = username,
                    ComentarioId = comentarioId,
                    Fecha = DateTime.UtcNow,
                    Accion = accion
                };

                _apreciacionesComentsCollection.InsertOne(apreciacion);
            }
        }


    }

}
