using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LoginAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace LoginAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly IConfiguration _configuration;

        public LoginController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        private string GenerateJwtToken(user user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("SecretKey"));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("username", user.username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(user user)
        {
            try
            {
                var existingUser = _context.users.FirstOrDefault(u => u.username == user.username);

                if (existingUser != null)
                {
                    // User already exists, check if the password is correct
                    if (existingUser.password == user.password)
                    {
                        // User is authenticated, generate JWT token
                        var token = GenerateJwtToken(existingUser);
                        return Ok(new { Token = token });
                    }
                    else
                    {
                        // Incorrect password
                        return Unauthorized("Incorrect password");
                    }
                }
                else
                {
                    // User doesn't exist
                    return Unauthorized("User not found");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{username}")]
        [Authorize]
        public IActionResult GetUserByUsername(string username)
        {
            try
            {
                var user = _context.users.FirstOrDefault(u => u.username == username);

                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}