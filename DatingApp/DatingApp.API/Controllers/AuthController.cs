using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dto;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepository _authRepository;

        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegister userToRegister)
        {
            userToRegister.UserName = userToRegister.UserName.ToLower();

            if (await _authRepository.UserExists(userToRegister.UserName))
                return BadRequest("User Name already exits");

            var user = new User
            { UserName = userToRegister.UserName };

            var createdUser = await _authRepository.Register(user, userToRegister.Password);

            return StatusCode(201, new {
                message = "User Registered Successfully"
            });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLogin userToLogin)
        {
            //throw new Exception("This is not Handled Exception");
            var loggedInUser = await _authRepository.Login(userToLogin.UserName.ToLower(), userToLogin.Password);

            if (loggedInUser == null)
                return Unauthorized();

            var claims = new[]
            {
              new Claim(ClaimTypes.NameIdentifier, loggedInUser.Id.ToString()),
              new Claim(ClaimTypes.Name, loggedInUser.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Authorization:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDesciptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDesciptor);

            return Ok(new { 
                token = tokenHandler.WriteToken(token),
                message = "Logged In Successull"
            });

        }
    }
}