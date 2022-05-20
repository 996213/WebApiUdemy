using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiUdemy.DTO;
using WebApiUdemy.Entities;

namespace WebApiUdemy.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //Fuente, a donde
            CreateMap<AutorCreacionDTO, Author>();
            CreateMap<Author, AutorResponseDTO>();
        }
    }
}
