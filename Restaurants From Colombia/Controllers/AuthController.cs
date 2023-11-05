using Microsoft.AspNetCore.Mvc;
using Restaurants_From_Colombia.Model;
using Restaurants_From_Colombia.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Restaurants_From_Colombia.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public AuthController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = _userService.Authenticate(userLogin.Username, userLogin.Password);

            if (user == null)
                return BadRequest(new { message = "Nombre de usuario o contraseña incorrectos" });

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])); // Utiliza UTF8 para garantizar que la clave tenga la longitud adecuada

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user._Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            // Agrega más reclamaciones según sea necesario
        }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["DurationInMinutes"])),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256) // Usa SecurityAlgorithms.HmacSha256
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }

}
