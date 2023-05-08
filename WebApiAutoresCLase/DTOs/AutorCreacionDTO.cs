using System.ComponentModel.DataAnnotations;

namespace WebApiAutoresCLase.DTOs
{
    public class AutorCreacionDTO
    {
        // En el modelo vemos y ponemos los campos que podemos ingresar
        // Y tambien le pondemos las anotaciones
        [Required(ErrorMessage = "El campo {0} es requerido")]//Entre (Mensaje  de error)
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} no debe de tener mas de {1} caracteres")] // Maximo de caracteres
        public string Nombre { get; set; }

        // con este vvamos a crear el autor en el metodo Post de controller
    }
}
