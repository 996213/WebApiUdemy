using AutoMapper;
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

        public CommentsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
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
        public async Task<ActionResult> Post(int libroId, CommentRequestDTO request)
        {
            var libroExiste = await context.Libros.FirstOrDefaultAsync(x => x.Id == libroId);
            if (libroExiste == null)
            {
                return NotFound($"El libro no existe");
            }

            var comentario = mapper.Map<Comments>(request);
            comentario.LibroId = libroId;

            context.Add(comentario);
            await context.SaveChangesAsync();
            return Ok();
        }
        

        
    }
}
