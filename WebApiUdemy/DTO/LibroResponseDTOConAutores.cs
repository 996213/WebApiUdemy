using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUdemy.DTO
{
    public class LibroResponseDTOConAutores : LibroResponseDTO
    {
        public List<AutorResponseDTO> Autores { get; set; }
    }
}
