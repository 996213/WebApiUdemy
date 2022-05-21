using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApiUdemy.Validations;

namespace WebApiUdemy.Entities
{
    public class Author : IValidatableObject
    {
        public int Id{ get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 5, ErrorMessage = "El campo {0} no debe tener {1} caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public List<BookAuthor> AutorLibro { get; set; }
        //[Range(minimum: 28, maximum:120)]
        //[NotMapped]
        //public int Edad { get; set; }
        //[CreditCard]
        //[NotMapped]
        //public string TarjetaCredito { get; set; }
        //[Url]
        //[NotMapped]
        //public string URLAutor { get; set; }
        //[NotMapped]
        //public int Menor { get; set; }
        //[NotMapped]
        //public int Mayor { get; set; }

        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre.ToString()[0].ToString();
                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula",
                        new String[] { nameof(Nombre) });
                }
                //if (Menor > Mayor)
                //{
                //    yield return new ValidationResult("El campo menor no puede ser mayor que el campo Mayor",
                //        new String[] { nameof(Nombre) });
                //}
                
            }
            
            

        }
    }
}
