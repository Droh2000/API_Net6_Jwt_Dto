using System.ComponentModel.DataAnnotations;

namespace WebApiAutoresCLase.DTOs
{
    public class CreacionUsuarioDTO:CredencialesUsuario
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public int numeroExterior { get; set; }
        public int numeroInteriro { get; set; }
    }
}
