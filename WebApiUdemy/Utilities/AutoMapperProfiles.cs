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

            CreateMap<LibroCreacionDTO, Book>()
                .ForMember(libro => libro.AutoresLibros
                , opciones => opciones.MapFrom(MapAutoresLibros));

            CreateMap<Book, LibroResponseDTO>()
                .ForMember(libro => libro.Autores
                , opciones => opciones.MapFrom(MapLibroResponseDTOBook));

            CreateMap<CommentRequestDTO, Comments>();
            CreateMap<Comments, CommentResponseDTO>();


        }

        private List<BookAuthor> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Book libro)
        {
            var resultado = new List<BookAuthor>();
            if (libroCreacionDTO.AutoresId == null)
                return resultado;

            foreach (var autorId in libroCreacionDTO.AutoresId )
            {
                resultado.Add(new BookAuthor() { AutorId = autorId });
            }
            return resultado;
        }

        private List<AutorResponseDTO> MapLibroResponseDTOBook(Book book, LibroResponseDTO libroDTO)
        {
            var resultado = new List<AutorResponseDTO>();
            if(book.AutoresLibros == null)
                return resultado;

            foreach (var autorLibro in book.AutoresLibros)
            {
                resultado.Add(new AutorResponseDTO()
                {
                    Id= autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                });
            }
            return resultado;
        }
    }
}
