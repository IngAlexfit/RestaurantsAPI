using MongoDB.Bson.Serialization.Attributes;

namespace Restaurants_From_Colombia.Model
{
    public class UserLogin
    {
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
    }
}
