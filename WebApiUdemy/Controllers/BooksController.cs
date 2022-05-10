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
    [Route("api/libros")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public BooksController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            return await context.Libros.Include(x=>x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Book libro)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);
            if (!existe)
            {
                return BadRequest($"No existe el autor con id: { libro.AutorId }");
            }
            context.Libros.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Book libro, int id)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == id);

            return Ok();
        }
    }
}
