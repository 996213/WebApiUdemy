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

        [HttpGet("{id:int}", Name = "ObtenerLibroPorId")]
        public async Task<LibroResponseDTOConAutores> Get(int id)
        {
            var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros)
                .ThenInclude(z=>z.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            libro.AutoresLibros.OrderBy(x => x.Orden).ToList();
            var data = mapper.Map<LibroResponseDTOConAutores>(libro);
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


            var LibroDTO = mapper.Map<LibroResponseDTO>(libro);
            AsignarOrdenAutores(libro);
            context.Libros.Add(libro);          
            await context.SaveChangesAsync();
            return CreatedAtRoute("ObtenerLibroPorId", new { id = libro.Id }, LibroDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(LibroCreacionDTO libroCreacionDTO, int id)
        {
            var existeLibro = await context.Libros
                .Include(x=>x.AutoresLibros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existeLibro == null)
                return NotFound();
            //context.Libros.Update(existeLibro);
            existeLibro = mapper.Map(libroCreacionDTO, existeLibro) ;
            AsignarOrdenAutores(existeLibro);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private void AsignarOrdenAutores(Book libro) {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i + 1;
                }
            }
        }
    }
}
