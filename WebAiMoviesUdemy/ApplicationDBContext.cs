using Microsoft.EntityFrameworkCore;
using WebApiMoviesUdemy.Entidades;

namespace WebApiMoviesUdemy
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }

    }
}
