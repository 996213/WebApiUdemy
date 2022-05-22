using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiUdemy.Entities;

namespace WebApiUdemy
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BookAuthor>()
                .HasKey(al => new { al.AutorId, al.LibroId });
        }

        public DbSet<Author> Autores { get; set; }
        public DbSet<Book> Libros { get; set; }
        public DbSet<Comments> Comentarios { get; set; }
        public DbSet<BookAuthor> AutoresLibros { get; set; }
    }
}
