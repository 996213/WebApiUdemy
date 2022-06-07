using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiUdemy.DTO;

namespace WebApiUdemy.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountsController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }


        [HttpPost("registrar")]
        public async Task<ActionResult<UserResponseDTO>> Registrar(UserRequestDTO userRequest)
        {
                var usuario = new IdentityUser { UserName = userRequest.Email, Email = userRequest.Email };
                var resultado = await userManager.CreateAsync(usuario, userRequest.Password);

            if (resultado.Succeeded)
            {
                return GenerarToken(userRequest);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }



        }
        [HttpPost("login")]

        public async Task<ActionResult<UserResponseDTO>> Login(UserRequestDTO userRequest)
        {
            var resultado = await signInManager.PasswordSignInAsync(userRequest.Email, userRequest.Password,
                isPersistent: false, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return GenerarToken(userRequest);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }
        }

        private UserResponseDTO GenerarToken(UserRequestDTO userRequest)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userRequest.Email),
                new Claim("lo que sea", "Dato no sensitivo")
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    configuration["llaveJwt"]
                ));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddYears(1);

            var tokenSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new UserResponseDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenSeguridad),
                Expiracion = expiracion
            };

        }

    }
}
