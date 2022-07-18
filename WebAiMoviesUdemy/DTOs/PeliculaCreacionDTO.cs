using System.ComponentModel.DataAnnotations;
using WebApiMoviesUdemy.Validaciones;

namespace WebApiMoviesUdemy.DTOs
{
    public class PeliculaCreacionDTO :PeliculaPatchDTO
    {

        [PesoArchivoValidacion(pesoMaximoEnBegaBytes:4)]
        [TipoArchivoValidacion(Enums.GrupoTipoArchivo.imagen)]
        public IFormFile Poster { get; set; }
    }
}
