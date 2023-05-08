using System.ComponentModel.DataAnnotations;

namespace WebApiAutoresCLase.DTOs
{
    // este es como el modelo esto es lo que vamos a recibir
    // es lo que pedimos
    public class CredencialesUsuario
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
