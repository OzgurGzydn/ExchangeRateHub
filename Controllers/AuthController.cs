using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExchangeRateHub.Models;

namespace ExchangeRateHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private static List<User> users = new List<User>();

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Username and password are required.");

            if (users.Any(x => x.Username == user.Username))
                return BadRequest("User already exists.");

            users.Add(user);

            return Ok("Registered successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            var existingUser = users.FirstOrDefault(x =>
                x.Username == user.Username &&
                x.Password == user.Password);

            if (existingUser == null)
                return Unauthorized("Invalid username or password.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ThisIsMySuperSecretJwtKeyForExchangeRateHub2026");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}