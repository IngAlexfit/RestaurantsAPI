using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Restaurants_From_Colombia.Model;
using Restaurants_From_Colombia.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
namespace Restaurants_From_Colombia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly RestauranteService _restauranteService;
        private readonly ApreciacionesComentsService _apreciacionesComentsService;
        private readonly IConfiguration _configuration;

        public ComentarioController(RestauranteService restauranteService,
                                    ApreciacionesComentsService apreciacionesComentsService,
                                    IConfiguration configuration)
        {
            _restauranteService = restauranteService;
            _apreciacionesComentsService = apreciacionesComentsService;
            _configuration = configuration;
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
            // Obtener token del encabezado
            var jwtToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")[1];

            if (jwtToken == null)
                return Unauthorized("Usuario no Autenticado");

            // Obtener key desde configuración
            var key = _configuration["JwtSettings:Key"];

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(jwtToken,
                         new TokenValidationParameters
                         {
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                             ValidateIssuer = false,
                             ValidateAudience = false
                         },
                         out SecurityToken validatedToken);

                
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
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message);
            }

        }

        // Clase de configuración
        public class JwtSettings
        {
            public string Key { get; set; }
            public int DurationInMinutes { get; set; }
        }
       

        [HttpPost("IncrementarLike")]
        public IActionResult IncrementarLike([FromBody] ComentarioIdModel comentarioIdModel)
        {
            // Obtener token del encabezado
            var jwtToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")[1];

            if (jwtToken == null)
                return Unauthorized("Usuario no Autenticado");

            // Obtener key desde configuración
            var key = _configuration["JwtSettings:Key"];

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(jwtToken,
                          new TokenValidationParameters
                          {
                              ValidateIssuerSigningKey = true,
                              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                              ValidateIssuer = false,
                              ValidateAudience = false
                          },
                          out SecurityToken validatedToken);

                // Obtener username del principal
                var username = principal.Identity.Name;
                // Convertir la cadena a DateTime
                DateTime creationTime = DateTime.Parse(comentarioIdModel.creationTime);

                var comentarioId = new ObjectId(
                  creationTime,
                  comentarioIdModel.machine,
                  (short)comentarioIdModel.pid, 
                  comentarioIdModel.increment
              );



                if (comentarioId == ObjectId.Empty)
                {
                    return BadRequest("ID de Comentario no válido");
                }

                if (_apreciacionesComentsService.UsuarioHaDadoLikeAComentario(username, comentarioId))
                {
                    return BadRequest(new { message = "El usuario ya ha dado like" });
                }

                var comentario = _restauranteService.GetComentariosById(comentarioId);

                if (comentario == null)
                {
                    return NotFound($"No encontrado comentario con ID {comentarioId}");
                }

                _restauranteService.IncrementarLike(comentarioId);

                _apreciacionesComentsService.RegistrarApreciacion(username, comentarioId);

                return Ok("Like incrementado y apreciación registrada");

            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message);
            }
        }









    }
}
