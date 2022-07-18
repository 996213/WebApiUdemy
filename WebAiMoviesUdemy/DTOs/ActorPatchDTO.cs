using System.ComponentModel.DataAnnotations;

namespace WebApiMoviesUdemy.DTOs
{
    public class ActorPatchDTO
    {
        [Required]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
