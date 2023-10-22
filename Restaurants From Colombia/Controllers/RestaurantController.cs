using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Restaurants_From_Colombia.BD;
using Restaurants_From_Colombia.Model;
using Restaurants_From_Colombia.Services;
using System.Collections.Generic;

namespace Restaurants_From_Colombia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly RestauranteService _restauranteService;

        public RestaurantController(RestauranteService restauranteService)
        {
            _restauranteService = restauranteService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurante>> Get()
        {
            var restaurantes = _restauranteService.GetRestaurantes();
            return Ok(restaurantes);
        }

        [HttpGet("byCiudad/{ciudad}")]
        public ActionResult<IEnumerable<Restaurante>> GetRestaurantesPorCiudad(string ciudad)
        {
            var restaurantes = _restauranteService.GetRestaurantesPorCiudad(ciudad);
            if (restaurantes == null || !restaurantes.Any())
            {
                return NotFound($"No se encontraron restaurantes en la ciudad de {ciudad}");
            }
            return Ok(restaurantes);
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurante> GetRestaurantePorId([FromRoute] int id)
        {
           

            var restaurante = _restauranteService.GetRestaurantePorId(id);

            if (restaurante == null)
            {
                return NotFound($"No se encontró un restaurante con el ID {id}");
            }

            return Ok(restaurante);
        }




    }
}
