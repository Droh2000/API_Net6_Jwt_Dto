using AutoMapper;
using WebApiAutoresCLase.DTOs;
using WebApiAutoresCLase.Models;

namespace WebApiAutoresCLase.Utilidades
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            // El Mapper Opuesto para mostrar solo lo que queramos mostrar
            CreateMap<Autor, AutorDto>().ReverseMap();
            // crea el mapper desde el DTO al modelo autor
            // y asi conectar con el DBcontext
            CreateMap<AutorCreacionDTO, Autor>();
            
            // Leer ORM entity
            // Generamos la lista de los libros por eso el metodo List<> de mas abajo
            // Todos los libros que tenga el autor
            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(x => x.Libros, opciones => opciones.MapFrom(MapAutorDTOLibro));

            // Mapeo Libros (Con la Relacion Muchos a Muchos con la Tabla AutorLibro) *********************************

            CreateMap<Libro, LibroDTO>();// Esto es para mostrar solo los datos que nos importa
            
            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(x => x.AutorLibro, options => options.MapFrom(MapeoAutorLibro));
            // Obtener Libros con Autores
            CreateMap<Libro, LibroDTOConAutores>()
                .ForMember(x => x.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));

            /* **************************************************************************************** */
            //CreateMap<ApplicationUser, CreacionUsuarioDTO>();
            CreateMap<CreacionUsuarioDTO, ApplicationUser>();
        }
        // Funcion en AutoresController para obtener sus diferentes libros
        private List<LibroDTO> MapAutorDTOLibro(Autor autor,AutorDto autorDto) 
        {
            var lista = new List<LibroDTO>();
            if (autor.AutorLibro == null) 
            {
                return lista;
            }

            foreach (var autorLibro in autor.AutorLibro)
            {
                lista.Add(new LibroDTO()
                {
                    id = autorLibro.LibroID,
                    Titulo = autorLibro.Libro.Titulo
                });
            }
            return lista;
        }

        // Funcion Para Ingresar los Diferentes Autores en POST de LibroController
        private List<AutorLibro> MapeoAutorLibro(LibroCreacionDTO libroCreacionDTO, Libro libro)
        {
            List<AutorLibro> response = new List<AutorLibro>();

            if (libroCreacionDTO.AutoresId == null)
                return response;

            foreach (int id in libroCreacionDTO.AutoresId)
            {
                response.Add(new AutorLibro { AutorID = id });
            }
            return response;
        }

        // Funcion en LibroController para obtener sus autores
        private List<AutorDto> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var lista = new List<AutorDto>();
            if (libro.AutorLibro == null)
            {
                return lista;
            }

            foreach (var autorLibro in libro.AutorLibro)
            {
                lista.Add(new AutorDto()
                {
                    id = autorLibro.AutorID,
                    Nombre = autorLibro.Autor.Nombre
                });
            }
            return lista;
        }
    }
}
