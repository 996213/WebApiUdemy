using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
        #region  -- Metodos Publicos --
        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "ObtenerLibroPorId")]
        public async Task<ActionResult<LibroResponseDTOConAutores>> Get(int id)
        {
            var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros)
                .ThenInclude(z=>z.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null)
                return NotFound();

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

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<BookPatchDTO> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();

            var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            if (libroDB == null)
                return NotFound();

            var libroDTO = mapper.Map<BookPatchDTO>(libroDB);
            patchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);
            if (!esValido)
                return BadRequest(ModelState);

            mapper.Map(libroDTO, libroDB);


            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Author { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
        #endregion

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
