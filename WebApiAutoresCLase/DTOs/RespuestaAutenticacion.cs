namespace WebApiAutoresCLase.DTOs
{
    public class RespuestaAutenticacion
    {
        // ESto es lo que vamos a mandar al frontend para saber si ya esta autentucado
        // es con lo que vamos a contestar
        public string token { get; set; }
        public DateTime expiracion { get; set; }
    }
}
