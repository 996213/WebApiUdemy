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
    [Route("api/libros")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroResponseDTO>> Get(int id)
        {
            var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros)
                .ThenInclude(z=>z.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);
            var data = mapper.Map<LibroResponseDTO>(libro);
            return data;
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroDTO)
        {
            if (libroDTO.AutoresId == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }

            var autoresIds = await context.Autores.Where(x => libroDTO.AutoresId.Contains(x.Id)).Select(x => x.Id).ToListAsync();

            if(libroDTO.AutoresId.Count != autoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }
            
            var libro = mapper.Map<Book>(libroDTO);
            if(libroDTO.AutoresId!= null)
            {
                for (int i = 0; i < libroDTO.AutoresId.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i + 1;
                }
            }


            context.Libros.Add(libro);          
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Book libro, int id)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == id);
            //context.Libros.Update(existeLibro);
            context.Update(libro);

            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
