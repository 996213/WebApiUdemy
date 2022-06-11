using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
                return await GenerarToken(userRequest);
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
                return  await GenerarToken(userRequest);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }
        }

        [HttpGet("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserResponseDTO>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var credencialesUsuario = new UserRequestDTO
            {
                Email = email
            };
             return await GenerarToken(credencialesUsuario);
        }

        [HttpPost("HacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }
        [HttpPost("RemoverAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

        private async Task<UserResponseDTO> GenerarToken(UserRequestDTO userRequest)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userRequest.Email),
                new Claim("lo que sea", "Dato no sensitivo")
            };

            var usuario = await userManager.FindByEmailAsync(userRequest.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    configuration["llaveJwt"]
                ));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddMinutes(30);

            var tokenSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new UserResponseDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenSeguridad),
                Expiracion = expiracion
            };

        }

    }
}
