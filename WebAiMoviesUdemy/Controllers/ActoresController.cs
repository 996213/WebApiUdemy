using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiMoviesUdemy.DTOs;
using WebApiMoviesUdemy.Entidades;

namespace WebApiMoviesUdemy.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : Controller
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public ActoresController(ApplicationDBContext context,
            IMapper mapper)
        {
            
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            var entidad =  await context.Actores.ToListAsync();
            return mapper.Map<List<ActorDTO>>(entidad);

        }

        [HttpGet("{id:int}", Name ="ObtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var entidad = await this.context.Actores.FirstOrDefaultAsync(a=>a.Id == id);
            if (entidad == null)
            {
                return NotFound($"No se encontro datos para el { id } suministrado");
            }
            return mapper.Map<ActorDTO>(entidad);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO dto)
        {
            var map = mapper.Map<Actor>(dto);
            context.Add(map);
            await context.SaveChangesAsync();
            var actorDto = mapper.Map<ActorDTO>(map);
            return new CreatedAtRouteResult("ObtenerActor", new { id = actorDto.Id }, actorDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var entidad = mapper.Map<Actor>(actorCreacionDTO);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Actores.AnyAsync(x => x.Id == id);
            if (!existe) return NotFound();

            context.Remove(new Genero() { Id = id });
            context.SaveChanges();
            return NoContent();

        }


    }
}
