using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace WebApiAutoresCLase.DTOs
{
    public class AgregarRol
    {
        // Solo le pedimos esto
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        // SE le puede agregar el Rol aqui para saber cual es
        // y mandarlo al controlador con un solo metodo y no crear un metodo por rol

    }
}
