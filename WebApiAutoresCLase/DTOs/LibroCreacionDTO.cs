namespace WebApiAutoresCLase.DTOs
{
    public class LibroCreacionDTO
    {
        // Solo queremos mostrar el nombre del libro
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        // SE pide la lista porque puede tener muchos autores
        public List<int> AutoresId { get; set; }
    }
}
