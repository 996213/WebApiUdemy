using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiMoviesUdemy.DTOs;
using WebApiMoviesUdemy.Entidades;

namespace WebApiMoviesUdemy.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public GenerosController(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            var entidades = await _context.Generos.ToListAsync(); 
            var dtos = _mapper.Map<List<GeneroDTO>>(entidades);

            return dtos;   
        }

        [HttpGet("{id:int}", Name = "ObtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            var entidad = await _context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if(entidad == null)
            {
                return NotFound("No se encontraron datos");
            }
            return _mapper.Map<GeneroDTO>(entidad);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO data )
        {

            var entidad = await _context.Generos.FirstOrDefaultAsync(x => x.Nombre.Contains(data.Nombre));
            if(entidad != null)
            {
                return BadRequest("Ya existe un registro con el mismo nombre");
            }
            var map = _mapper.Map<Genero>(data);
            _context.Add(map);
            _context.SaveChanges();
            var generoDTO = _mapper.Map<GeneroDTO>(map);
            return new CreatedAtRouteResult("ObtenerGenero", new { id = generoDTO.Id }, generoDTO);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put (int id, [FromBody] GeneroCreacionDTO generoCreacionDTO){
            var entidad = _mapper.Map<Genero>(generoCreacionDTO);
            entidad.Id = id;
            _context.Entry(entidad).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete (int id)
        {
            var existe = await _context.Generos.AnyAsync(x => x.Id == id);
            if (!existe) return NotFound();

            _context.Remove(new Genero() { Id = id });
            _context.SaveChanges();
            return NoContent();

        }



    }
}
