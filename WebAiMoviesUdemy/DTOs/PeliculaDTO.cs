namespace WebApiMoviesUdemy.DTOs
{
    public class PeliculaDTO
    {
        public int Id { get; set; }        
        public int Titulo { get; set; }

        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        public string Poster { get; set; }
    }
}
