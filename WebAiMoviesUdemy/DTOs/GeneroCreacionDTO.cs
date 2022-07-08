using System.ComponentModel.DataAnnotations;

namespace WebApiMoviesUdemy.DTOs
{
    public class GeneroCreacionDTO
    {
        [StringLength(40)]
        public string Nombre { get; set; }
    }
}
