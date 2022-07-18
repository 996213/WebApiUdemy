using System.ComponentModel.DataAnnotations;

namespace WebApiMoviesUdemy.DTOs
{
    public class PeliculaPatchDTO
    {
        [Required]
        [StringLength(300)]
        public int Titulo { get; set; }

        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
    }
}
