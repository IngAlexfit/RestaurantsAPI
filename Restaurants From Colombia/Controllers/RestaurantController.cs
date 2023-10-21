using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Restaurants_From_Colombia.BD;
using Restaurants_From_Colombia.Model;
namespace Restaurants_From_Colombia.BD
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IMongoCollection<Restaurante> _restaurantesCollection;

        public RestaurantController(MongoDBSettings mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.MongoDBConnection);
            var database = client.GetDatabase("restaurantManagement");
            _restaurantesCollection = database.GetCollection<Restaurante>("restaurantes");
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurante>> Get()
        {
            var restaurantes = _restaurantesCollection.Find(r => true).ToList();
            return Ok(restaurantes);
        }

        
    }
}