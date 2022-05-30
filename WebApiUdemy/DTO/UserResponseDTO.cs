using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUdemy.DTO
{
    public class UserResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
