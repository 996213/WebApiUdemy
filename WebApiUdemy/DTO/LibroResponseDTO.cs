using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUdemy.DTO
{
    public class LibroResponseDTO
    {        
        public string Titulo { get; set; }

        public List<AutorResponseDTO> Autores{ get; set; }
    }
}
