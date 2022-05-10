using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiUdemy.Entities;

namespace WebApiUdemy.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AuthorsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> Get()
        {
            return await context.Autores.Include(x=>x.Libros).ToListAsync();
        }

        [HttpGet("primero")]
        public async Task<ActionResult<Author>> PrimerAutor()
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}/{param?}")]
        [HttpGet("{id:int}/{param=2}")]
        public async Task<ActionResult<Author>> Get(int id, string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
                return NotFound();
            
            return autor;
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<Author>> Get(string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (autor == null)
                return NotFound();
            
            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Author autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")] //api/autores/2 PARAMETRO DE RUTA
        public async Task<ActionResult> Put (Author autor, int id)
        {
            if(id != autor.Id)
            {
                return BadRequest();
            }
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Author { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
