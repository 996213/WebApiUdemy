using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiUdemy.Validations;

namespace WebApiUdemy.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, ErrorMessage = "El campo {0} no debe tener {1} caracteres")]
        [PrimeraLetraMayuscula]
        public string Titulo { get; set; }
        public int AutorId { get; set; }
        //public Author Autor { get; set; }
        public List<Comments> Comentarios { get; set; }
        public List<BookAuthor> AutoresLibros { get; set; }
    }
}
