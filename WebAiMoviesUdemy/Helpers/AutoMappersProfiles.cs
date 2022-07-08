using AutoMapper;
using WebApiMoviesUdemy.DTOs;
using WebApiMoviesUdemy.Entidades;

namespace WebApiMoviesUdemy.Helpers
{
    public class AutoMappersProfiles : Profile
    {
        public AutoMappersProfiles()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>();
        }
    }
}
