using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiUdemy.Entities;
using WebApiUdemy.Services;

namespace WebApiUdemy.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        //private readonly ILogger logger;

        public AuthorsController(ApplicationDbContext context, IServicio servicio, ServicioTransient servicioTransient, ServicioScoped servicioScoped, ServicioSingleton servicioSingleton)
        {
            this.context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
            //this.logger = logger;
        }

        [HttpGet("GUID")]
        public ActionResult Guid()
        {
            return Ok(new
            {
                ControllerTransient = servicioTransient.Guid,
                ServicioTransient = servicio.ObtenerTransient(),
                ControllerScoped = servicioScoped.Guid,
                ServicioScoped = servicio.ObtenerScoped(),
                ControllerSingleton = servicioSingleton.Guid,                               
                ServicioSingleton = servicio.ObtenerSingleton()
            });;
        }

        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listado")]
        public async Task<ActionResult<List<Author>>> Get()
        {
            //logger.LogInformation("Obtener listado de autores");
            servicio.RealizarTarea();
            return await context.Autores.Include(x=>x.Libros).ToListAsync();
        }

        [HttpGet("primero")]
        public async Task<ActionResult<Author>> PrimerAutor()
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}/{param?}")]
        //[HttpGet("{id:int}/{param=2}")]
        public async Task<ActionResult<Author>> Get(int id, string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
                return NotFound();
            
            return autor;
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<Author>> Get([FromRoute] string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (autor == null)
                return NotFound();
            
            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Author autor)
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
