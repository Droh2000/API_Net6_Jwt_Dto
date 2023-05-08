namespace WebApiAutoresCLase.Models
{
    public class Libro
    {
        public int id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }

        //relacion Muchos a muchos
        public List<AutorLibro> AutorLibro { get; set; }
    }
}
