using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiUdemy.Validations;

namespace WebApiUdemy.DTO
{
    public class AutorCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 5, ErrorMessage = "El campo {0} no debe tener {1} caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        
    }
}
