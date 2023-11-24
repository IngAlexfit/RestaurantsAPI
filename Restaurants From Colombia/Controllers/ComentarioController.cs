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
        private readonly ComentsService _comentsService;
        private readonly IConfiguration _configuration;

        public ComentarioController(RestauranteService restauranteService,
                                    ComentsService comentsService,
                                    IConfiguration configuration)
        {
            _restauranteService = restauranteService;
            _comentsService = comentsService;
            _configuration = configuration;
        }

        [HttpGet("ByRestauranteId/{restauranteId}")]
        public ActionResult<IEnumerable<Comentario>> GetComentariosPorRestaurante(int restauranteId)
        {
            
            var restaurante = _restauranteService.GetComentariosPorRestaurante(restauranteId);
            return Ok(restaurante);
        }



        [HttpPost("agregar-comentario")]
        public IActionResult AgregarComentario([FromBody] ComentarioRegisterModel comentario)
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
                 
                var colombiaTz = TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");

                
                var comentarioG = new Comentario
                {
                    Autor = comentario.Autor,
                    Contenido = comentario.Contenido,
                    restaurante_id = comentario.restaurante_id,

                    // Convertir fecha UTC a zona horaria Colombia
                    Fecha = TimeZoneInfo.ConvertTime(DateTime.UtcNow, colombiaTz)

                };


                _comentsService.AgregarComentario(comentarioG);

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
                var comentario = _restauranteService.GetComentariosById(comentarioId);

                if (comentario == null)
                {
                    return NotFound($"No encontrado comentario con ID {comentarioId}");
                }
                if (_comentsService.UsuarioHaDadoDiskLikeAComentario(username, comentarioId))
                {
                    _comentsService.DecrementarDiskLike(comentarioId);
                }

                if (_comentsService.UsuarioHaDadoLikeAComentario(username, comentarioId))
                {
                    var accion1 = "like";

                    _comentsService.DecrementarLike(comentarioId);

                    _comentsService.EliminarApreciacion(username, comentarioId, accion1);

                    return Ok(new { message = "Has quitado tu like" });



                }
                

               



                var accion = "like";

                _comentsService.IncrementarLike(comentarioId);

                _comentsService.RegistrarApreciacion(username, comentarioId, accion);

                return Ok("like y apreciación registrada");

            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message);
            }
        }


        [HttpPost("DarDisklike")]
        public IActionResult DarDisklike([FromBody] ComentarioIdModel comentarioIdModel)
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
                var comentario = _restauranteService.GetComentariosById(comentarioId);

                if (comentario == null)
                {
                    return NotFound($"No encontrado comentario con ID {comentarioId}");
                }
                if (_comentsService.UsuarioHaDadoLikeAComentario(username, comentarioId))
                {
                    _comentsService.DecrementarLike(comentarioId);


                }

                if (_comentsService.UsuarioHaDadoDiskLikeAComentario(username, comentarioId))
                {
                    var accion1 = "dislike";

                    _comentsService.DecrementarDiskLike(comentarioId);

                    _comentsService.EliminarApreciacion(username, comentarioId, accion1);

                    return Ok(new { message = "Has quitado tu dislike" });
                }

               

                

                var accion = "dislike";

                _comentsService.IncrementarDiskLike(comentarioId);

                _comentsService.RegistrarApreciacion(username, comentarioId,accion);

                return Ok("Dislike  y apreciación registrada");

            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message);
            }
        }








    }
}
