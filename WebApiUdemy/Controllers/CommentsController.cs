using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiUdemy.DTO;
using WebApiUdemy.Entities;

namespace WebApiUdemy.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public CommentsController(ApplicationDbContext context, IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("{id:int}", Name = "ObtenerComentariosPorID")]
        public async Task<ActionResult<CommentResponseDTO>> GetbyId(int id)
        {
            var comentarioExiste = await context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if (comentarioExiste == null)
                return NotFound("El comentario no existe");

            return mapper.Map<CommentResponseDTO>(comentarioExiste);
        }

        [HttpGet("comentariosporlibro")]
        public async Task<ActionResult<List<CommentResponseDTO>>> Get(int libroId)
        {
            var libroExiste = await context.Libros.FirstOrDefaultAsync(x => x.Id == libroId);
            if (libroExiste == null)
            {
                return NotFound($"El libro no existe");
            }

            var comentarios = await context.Comentarios.Where(x => x.LibroId == libroId).ToListAsync();
            return mapper.Map<List<CommentResponseDTO>>(comentarios);


        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId, CommentRequestDTO request)
        {

            //Acceso a los claims del usuario a traves de JWT
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await _userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;
            var libroExiste = await context.Libros.FirstOrDefaultAsync(x => x.Id == libroId);
            if (libroExiste == null)
            {
                return NotFound($"El libro no existe");
            }

            var comentario = mapper.Map<Comments>(request);
            comentario.LibroId = libroId;
            comentario.UsuarioId = usuarioId;
            context.Add(comentario);
            await context.SaveChangesAsync();
            var ComentariosDTO = mapper.Map<CommentResponseDTO>(comentario);
            return CreatedAtRoute("ObtenerComentariosPorID", new { id = comentario.Id, libroId = libroId }, ComentariosDTO);
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CommentRequestDTO request)
        {
            var existeComentario = await context.Comentarios.AnyAsync(x => x.Id == id);
            if (!existeComentario)
                return NotFound();

            var comentario = mapper.Map<Comments>(request);
            comentario.Id = id;
            comentario.LibroId = 1;
            context.Update(comentario);
            await context.SaveChangesAsync();

            return NoContent();            

        }

        
    }
}
