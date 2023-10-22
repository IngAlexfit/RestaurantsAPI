﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Restaurants_From_Colombia.Model;
using Restaurants_From_Colombia.Services;
using System;

namespace Restaurants_From_Colombia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly RestauranteService _restauranteService;

        public ComentarioController(RestauranteService restauranteService)
        {
            _restauranteService = restauranteService;
        }


        [HttpGet("ByRestauranteId/{restauranteId}")]
        public ActionResult<IEnumerable<Comentario>> GetComentariosPorRestaurante(int restauranteId)
        {
            
            var restaurante = _restauranteService.GetComentariosPorRestaurante(restauranteId);
            return Ok(restaurante);
        }



        [HttpPost("agregar-comentario")]
        public IActionResult AgregarComentario([FromBody] Comentario comentario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Comprueba si el restaurante con el ID especificado existe
            var restauranteExistente = _restauranteService.GetRestaurantePorId(comentario.restaurante_id);
            if (restauranteExistente == null)
            {
                return Ok(comentario.restaurante_id);
            }
            _restauranteService.AgregarComentario(comentario);

            return Ok("Comentario agregado con éxito"); 
        }








    }
}