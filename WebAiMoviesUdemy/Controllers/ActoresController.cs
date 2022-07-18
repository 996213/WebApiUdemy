using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiMoviesUdemy.DTOs;
using WebApiMoviesUdemy.Entidades;
using WebApiMoviesUdemy.Helpers;
using WebApiMoviesUdemy.Servicios;

namespace WebApiMoviesUdemy.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : Controller
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor ="actores";


        public ActoresController(ApplicationDBContext context,
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            //Agrega a la cabecera HTTP la cantidad de paginas a la respuesta del usuario
            var queryable = context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);

            //Aplicar paginacion a nivel de EntityFreamwork
            var entidad =  await queryable.Paginar(paginacionDTO).ToListAsync();
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
            var entidad = mapper.Map<Actor>(dto);

            if (dto.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await dto.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extenision = Path.GetExtension(dto.Foto.FileName);
                    entidad.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extenision, contenedor,
                        dto.Foto.ContentType
                        );
                }
            }
            
            context.Add(entidad);
            await context.SaveChangesAsync();
            var actorDto = mapper.Map<ActorDTO>(entidad);
            return new CreatedAtRouteResult("ObtenerActor", new { id = actorDto.Id }, actorDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actorDB == null)
                return NotFound();
            //Mapea campos de actorCreacionDTO hacia la entidad: Los campos distintos son actualizados.
            actorDB = mapper.Map(actorCreacionDTO, actorDB);

            if (actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extenision = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actorDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extenision, contenedor,
                        actorDB.Foto,
                        actorCreacionDTO.Foto.ContentType
                        );
                }
            }

            //var entidad = mapper.Map<Actor>(actorCreacionDTO);
            //entidad.Id = id;
            //context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Actores.AnyAsync(x => x.Id == id);
            if (!existe) return NotFound();

            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch (int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();

            var entidadDB = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if(entidadDB == null)
                return NotFound();

            var entidadDTO = mapper.Map<ActorPatchDTO>(entidadDB);
            patchDocument.ApplyTo(entidadDTO, ModelState);
            var esValido = TryValidateModel(entidadDTO);
            if (!esValido)
                return BadRequest(ModelState);

            mapper.Map(entidadDTO, entidadDB); // Al pasar datos a la entidad, Entity Freamwork determina su edición
            await context.SaveChangesAsync();
            return NoContent();

        }


    }
}
