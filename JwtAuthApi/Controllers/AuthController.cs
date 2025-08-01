using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simulación de usuario y roles
            if (request.Username == "admin" && request.Password == "admin")
            {
                var roles = new[] { "Admin", "User" };
                var token = GenerateJwtToken(request.Username, roles);
                return Ok(new { token });
            }
            if (request.Username == "user" && request.Password == "user")
            {
                var roles = new[] { "User" };
                var token = GenerateJwtToken(request.Username, roles);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(string username, string[] roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKey@sadaasdsadasdsxasxasxasfdsabyytbytbt"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "JwtAuthApi",
                audience: "JwtAuthApi",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("admin")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return Ok("Solo los administradores pueden ver esto.");
        }

        [HttpGet("user")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult User()
        {
            return Ok("Cualquier usuario autenticado puede ver esto.");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
