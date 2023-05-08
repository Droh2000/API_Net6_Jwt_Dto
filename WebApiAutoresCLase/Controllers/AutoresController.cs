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
    public class AutoresController : Controller
    {
        //CReaacion el contexto
        private readonly AplicationDbContext _context;
        // Para usar el Mappeo (Configurar en el Program que vamos a usar AutoMapper)
        private readonly IMapper _mapper;

        public AutoresController(AplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Metodo para agregar un autor a la bae de datos
        // En el DBContext tenermos los modelos para conectarse con la BD
        // Al usar el modelo queda expuesto la informacion y para eso usamos DTOS
        // este DTO no esta ligado a un DBContext (No puede enviar info a la BD)
        // Asi que se tienen que mappear al modelo para asi insertar a la BD
        // Para que el DTO tenga los permisos en la BD
        // SE puede mapear de Dto a Modelo y de Modelo a DTO para no exponer los datos y mandarlos a la Bd

        // Para el mapeo se creo una carpeta utilitdades
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO) // Aqui see usa DTOs ya creado en la carpeta dto
        {
            // Busacr el autro para asegurar que no exista
            var existeAutor = await _context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);

            if (existeAutor) {
                // otra foram de concatenar es con $ para usa {}
                return BadRequest($"Autor {autorCreacionDTO.Nombre} duplicado");

            }

            // Usamos el Mappeo (Que se valla a autor lo que tengamos en el dto)
            var autor = _mapper.Map<Autor>(autorCreacionDTO);
            _context.Add(autor);
            await _context.SaveChangesAsync();

            // Aqui retorna los datos de mas
            //return Ok(autor);

            var autorDTO = _mapper.Map<AutorDto>(autor);
            return Ok(autorDTO);

            /*
                 autore
                Try it
                Ingresa en json
                No queremos que nos expnga toda la entidad asi que vamos a crear otro DTO con los campos que si
                queremos que nos retorne (Id y Nombre) AutorDTO

                Crear Dto AutorDto
                    Solo viene lo que queremos exponer (ID y Nombre)
                en automammerProfile y Hacemos lo contrario Map<Origen,Destino>
                Lo tenemos en <Autor,AutorDto>
        
                Ahora se hacemos lo mismo de las variables de arriba: var autor
            */
        }

        // Get para obtener una lista de autores y un usuario
        [HttpGet("primero")] //Aqui solo obtenemos el primero de la lista
        // queremos un autor DTO
        /*
            metodo
            async para evitar cuello de botella
            toda tarea se trae una tarea de actionresult
            (Este regresa cualquier valor que nececitemos)
         */
        public async Task<ActionResult<AutorDto>> primerAutor() {
            var autor = await _context.Autores.FirstOrDefaultAsync();// Primero de la lista
            // le mandamos autor pero espera un dto
            // entonces hacemos un mapeo
            return _mapper.Map<AutorDto>(autor);
        }

        /************     Separa Nombre e Id        ************/
        // Ahora pero pasandole el id del autor a obtener
        /*[HttpGet("{id:int}/{nombre?}", Name = "ObtenerAutor")]//Vamos a recibir un id en el endpoint, este Name se pone porque ya pusimos un parametro
        // Para tener mas de un parametro: se pone /{}
        // Al mandar String se pone '?' o si no da error
        public async Task<ActionResult<List<AutorDto>>> Get(int id, string nombre) {
            //va a la tabla autores y manda el primero que encuentre en el que el id de parametro sea igual al de tabla
            var autor = await _context.Autores.FirstOrDefaultAsync(x => x.id == id);
            var autor2 = await _context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();//este Where simpre toma una lista por eso se convierte a List el metodo y list

            if (autor == null) {
                return NotFound();
            }

            return _mapper.Map<List<AutorDto>>(autor);//Con el action result podemos mandar diferentes cosas
        }*/

        [HttpGet("{id:int}", Name = "ObtenerAutor")]
        // Ahora que nos retorne EL nuevo dto creado por la relacion
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {
            // Al buscar un autor que nos regreese tambien los libres que si tiene
            // Innejoint (Autor,libre y autorlibro)
            var autor = await _context.Autores
                .Include(x => x.AutorLibro)// inner join con la relacion TRaemos la tabla autorLibre
                .ThenInclude(x => x.Libro)
                .FirstOrDefaultAsync(x => x.id == id);
            // Aqui no sale la realcion
            // Relacionamos el dto , que solo tienen Id,nombre y entonces se tiene que crear uno nuevo
            // LibroDto
            // AutorDTOConLibros
            // Poner en el Automapper

            if (autor == null)
            {
                return NotFound();
            }

            return _mapper.Map<AutorDTOConLibros>(autor);
        }

        // Que modifique todos
        [HttpPut("{id:int}")] // dentrod e los (Viene de parametro en la URl), nececitamos una URl
        // En eset caso como son los mismo campos no tiene caso crear un Dto para modificar
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            //VErificar si existe
            var exist = await _context.Autores.AnyAsync(x => x.id == id);
            if (!exist) {
                return NotFound();
            }

            // DEl Dto lo mandamos al autor
            // El dto solo tiene el nombre y nos falta tener e id para tenrlo completo
            var autor = _mapper.Map<Autor>(autorCreacionDTO);
            autor.id = id; // En el post no se pone porque se crea solo
            _context.Update(autor); // LE decimos que modifique
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")] //nececitamos un id
        public async Task<ActionResult> Delete(int id) 
        {
            //VErificar si existe
            var exist = await _context.Autores.AnyAsync(x => x.id == id);
            if (!exist)
            {
                return NotFound();
            }

            //aqui se le pasa un autor, en este caso no lo tenemos porlo que se crea dentro
            _context.Remove(new Autor { id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
