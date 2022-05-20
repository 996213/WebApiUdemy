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
            var libro = await context.Libros.Include(x=>x.Autor).FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<LibroResponseDTO>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroDTO)
        {
            //var existe = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);
            //if (!existe)
            //{
            //    return BadRequest($"No existe el autor con id: { libro.AutorId }");
            //}
            var libro = mapper.Map<Book>(libroDTO);
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
