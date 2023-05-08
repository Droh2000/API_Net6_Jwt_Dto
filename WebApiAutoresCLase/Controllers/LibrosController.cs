using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutoresCLase.Data;
using WebApiAutoresCLase.DTOs;
using WebApiAutoresCLase.Models;

namespace WebApiAutoresCLase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController:Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IMapper _mapper;

        public LibrosController(AplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Al crear el libro tiene que existir el autor
        [HttpPost]
        public async Task<ActionResult> CrearLibro([FromBody] LibroCreacionDTO libroCreacionDTO) 
        {
            if (libroCreacionDTO.AutoresId == null)
                return BadRequest("No se puede insertar un libro sin asignarle al menos un autor");

            // Busacar el autor para asegurar que exista
            foreach (var autor in libroCreacionDTO.AutoresId)
            {
                var existeAutor = await _context.Autores.AnyAsync(x => x.id == autor);

                if (!existeAutor)
                {
                    // otra foram de concatenar es con $ para usa {}
                    return BadRequest($"Autor {libroCreacionDTO.AutoresId} duplicado");
                }
            }
            // Buscar el Libro para verificar que no exista
            var existeLibro = await _context.Libros.AnyAsync(x => x.Titulo == libroCreacionDTO.Titulo);

            if (existeLibro)
            {
                // otra foram de concatenar es con $ para usa {}
                return BadRequest($"Libro {libroCreacionDTO.Titulo} duplicado");
            }
            // Usamos el Mappeo (Que se valla al modelo Libro lo que tengamos en el Dto)
            var libro = _mapper.Map<Libro>(libroCreacionDTO);
            _context.Add(libro);
            await _context.SaveChangesAsync();
            // SE tiene que pasa Id del libro y Id de autores
            //_context.Add(_mapper.Map<AutorLibro>(libroCreacionDTO));
            //_context.Add(_mapper.Map<AutorLibro>(libro));
            //await _context.SaveChangesAsync();

            // Este es solo para maostrar los datos
            var libroDTO = _mapper.Map<LibroDTO>(libro);
            return Ok(libroDTO);
        }

        /*private void AsignarOrdenAutores(Libro libro)
        {
            for (int i = 0; i < libro.AutorLibro.Count; i++) 
            {
                libro.AutorLibro. = i;
            }
        }*/

        [HttpGet("primerLibro")]
        public async Task<ActionResult<LibroDTO>> primerLibro()
        {
            var libro = await _context.Libros.FirstOrDefaultAsync();// Primero de la lista
            return _mapper.Map<LibroDTO>(libro);
        }

        // Obtener Libro por Nombre (Solo obtenemos los detalles pero sin los Autores)
        [HttpGet("{nombre?}", Name = "ObtenerLibro")]
        public async Task<ActionResult<List<LibroDTO>>> Get(string nombre)
        {
            var libro = await _context.Libros.Where(x => x.Titulo.Contains(nombre)).ToListAsync();
            if (libro == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<LibroDTO>>(libro);
        }

        // Obtener libro con los autores
        [HttpGet("{id:int}", Name = "ObtenerLibroAutores")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            var libro = await _context.Libros
                .Include(x => x.AutorLibro)// inner join Traemos la tabla autorLibro (Relaciona las dos tablas)
                .ThenInclude(x => x.Autor) // Queremos el Autor
                .FirstOrDefaultAsync(x => x.id == id);

            if (libro == null)
            {
                return NotFound();
            }
            return _mapper.Map<LibroDTOConAutores>(libro);
        }

        // Modificar Libro (Recardar la Relacion de muchos a muchos)
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(LibroCreacionDTO libroCreacionDTO, int id)
        {
            var exist = await _context.Libros.Include(x => x.AutorLibro).FirstOrDefaultAsync(x => x.id == id); //así traigo el prospecto y también sus agentes
            if (exist==null)
            {
                return NotFound();
            }

            var libro = _mapper.Map(libroCreacionDTO, exist); //mapeando objetos existentes de origen y destino
            //libro.id = id;

            _context.Libros.Update(libro);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Eliminar Libro
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await _context.Libros.AnyAsync(x => x.id == id);
            if (!exist)
            {
                return NotFound();
            }

            _context.Remove(new Libro { id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
