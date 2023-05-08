using System.ComponentModel.DataAnnotations;

namespace WebApiAutoresCLase.Models
{
    public class Autor
    {
        public int id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido")]//Entre (Mensaje  de error)
        [StringLength(maximumLength:20, ErrorMessage = "El campo {0} no debe de tener mas de {1} caracteres")] // Maximo de caracteres
        public string Nombre { get; set; }

        //relacion muchos a muchos de autor libro (se requiere hacer otra tabla) (Modelo)
        public List<AutorLibro> AutorLibro { get; set; }
    }
}
