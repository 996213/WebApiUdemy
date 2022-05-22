using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUdemy.DTO
{
    public class AutorResponseDTOConLibros : AutorResponseDTO
    {
        public List<LibroResponseDTO> Libros { get; set; }
    }
}
