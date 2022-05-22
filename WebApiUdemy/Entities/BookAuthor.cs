using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUdemy.Entities
{
    public class BookAuthor
    {
        public int LibroId { get; set; }
        public int AutorId { get; set; }
        public int Orden { get; set; }
        public Book Libro { get; set; }
        public Author Autor { get; set; }

    }
}
