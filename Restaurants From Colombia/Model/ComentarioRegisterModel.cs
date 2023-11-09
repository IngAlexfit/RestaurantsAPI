using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Restaurants_From_Colombia.Model
{
    public class ComentarioRegisterModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [Required(ErrorMessage = "El campo 'autor' es obligatorio.")]
        [BsonElement("autor")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "El campo 'contenido' es obligatorio.")]
        [BsonElement("contenido")]
        public string Contenido { get; set; }

        [BsonElement("likes")]
        public int Likes { get; set; } = 0;

        [BsonElement("dislikes")]
        public int Dislikes { get; set; } = 0;

        [Required(ErrorMessage = "El campo 'RestauranteId' es obligatorio.")]
        [BsonElement("restaurante_id")]
        public int restaurante_id { get; set; }
    }
}
