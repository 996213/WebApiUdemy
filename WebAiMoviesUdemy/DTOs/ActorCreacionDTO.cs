using WebApiMoviesUdemy.Enums;
using WebApiMoviesUdemy.Validaciones;

namespace WebApiMoviesUdemy.DTOs
{
    public class ActorCreacionDTO
    {
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [PesoArchivoValidacion(pesoMaximoEnBegaBytes: 4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.imagen )]
        public IFormFile Foto { get; set; }
    }
}
