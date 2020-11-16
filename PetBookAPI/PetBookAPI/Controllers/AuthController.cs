using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetBookAPI.DataTransferFiles;
using PetBookAPI.Model;

namespace PetBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthorizationRepository authorizationRepository;
        private readonly IConfiguration configuration;

        public AuthController(IAuthorizationRepository authorizationRepository, IConfiguration configuration)
        {
            this.authorizationRepository = authorizationRepository;
            this.configuration = configuration;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register(Pet pet)
        {
            if (authorizationRepository.Exists(pet.Name))
            {
                return BadRequest("Użytkownik o tej nazwie już istnieje. Wybierz inną.");
            }

            var createdPet = await authorizationRepository.Register(pet);
            return StatusCode(201);
            //return CreatedAtRoute();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Pet pet)
        {
            var loggedPet = await authorizationRepository.Login(pet.Name, pet.Password);

            if (loggedPet == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loggedPet.Id.ToString()),
                new Claim(ClaimTypes.Name, loggedPet.Name)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(3),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesc);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }

    }
}