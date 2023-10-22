namespace Restaurants_From_Colombia.Model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Restaurante
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _Id { get; set; }

        [BsonElement("restaurant_id")]
        public int IdRestaur { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("ubicacion")]
        public string Ubicacion { get; set; }

        [BsonElement("tipo_cocina")]
        public string TipoCocina { get; set; }

        [BsonElement("descripcion")]
        public string Descripcion { get; set; }

        [BsonElement("imagen_url")]
        public string ImagenUrl { get; set; }

        [BsonElement("telefono")]
        public string Telefono { get; set; }

        [BsonElement("horario")]
        public string Horario { get; set; }

        [BsonElement("likes")]
        public int Likes { get; set; }
    }
}
