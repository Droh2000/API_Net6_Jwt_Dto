using System.ComponentModel.DataAnnotations;

namespace WebApiAutoresCLase.DTOs
{
    public class AutorDto
    {
        // aqui lo mapeamos de algo que ya esta en la bd por eso no se pone los de
        // caracteres
        public int id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]//Entre (Mensaje  de error)
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} no debe de tener mas de {1} caracteres")] // Maximo de caracteres
        public string Nombre { get; set; }
    }
}
