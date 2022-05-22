using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiUdemy.DTO;
using WebApiUdemy.Entities;
using WebApiUdemy.Filter;
using WebApiUdemy.Services;

namespace WebApiUdemy.Controllers
{
    [ApiController]
    [Route("api/authors")]
    //[Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public AuthorsController(ApplicationDbContext context, IServicio servicio, ServicioTransient servicioTransient, ServicioScoped servicioScoped, 
            ServicioSingleton servicioSingleton, ILogger<AuthorsController> logger, IMapper mapper)
        {
            this.context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet("GUID")]
        //[ResponseCache(Duration =10)]
        [ServiceFilter(typeof(MiFiltroDeAccion))]
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
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public async Task<List<AutorResponseDTO>> Get()
        {
            logger.LogInformation("Obtener listado de autores");
            servicio.RealizarTarea();            

            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorResponseDTO>>(autores);
        }

        [HttpGet("primero")]
        public async Task<ActionResult<Author>> PrimerAutor()
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}/{param?}", Name = "ObtenerAutorPorId")]
        //[HttpGet("{id:int}/{param=2}")]
        public async Task<ActionResult<AutorResponseDTOConLibros>> Get(int id, string nombre)
        {
            var autor = await context.Autores
                .Include(autorDb=>autorDb.AutorLibro)
                .ThenInclude(autorlibro => autorlibro.Libro)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
                return NotFound();
            
            return mapper.Map<AutorResponseDTOConLibros>(autor);
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<Author>>> Get([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();
            if (autores == null)
                return NotFound();
            

            return autores;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var existeConElMismoNombre = await context.Autores.AnyAsync(x=>x.Nombre == autorCreacionDTO.Nombre);
            if (existeConElMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre ${ autorCreacionDTO.Nombre }");
            }

            var autor = mapper.Map<Author>(autorCreacionDTO);

                context.Add(autor);
            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorResponseDTO>(autor);

            return  CreatedAtRoute("ObtenerAutorPorId", new { id = autor.Id }, autorDTO);
        }

        [HttpPut("{id:int}")] //api/autores/2 PARAMETRO DE RUTA
        public async Task<ActionResult> Put (AutorCreacionDTO autorCreacionDTO, int id)
        {
            
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            var autor = mapper.Map<Author>(autorCreacionDTO);
            autor.Id = id;
            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
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
