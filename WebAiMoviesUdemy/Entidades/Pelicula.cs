using System.ComponentModel.DataAnnotations;

namespace WebApiMoviesUdemy.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public int Titulo { get; set; }

        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        public string Poster { get; set; }

    }
}
