using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Restaurants_From_Colombia.Model
{
    public class ApreciacionComentario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _Id { get; set; }

        [BsonElement("usuario")]
        public string Usuario { get; set; }

        [BsonElement("comentario_id")]
        public ObjectId ComentarioId { get; set; }

        [BsonElement("fecha")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Fecha { get; set; }

        [BsonElement("accion")]
        public string Accion { get; set; }

    }
}
