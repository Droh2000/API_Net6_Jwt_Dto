namespace WebApiAutoresCLase.Models
{
    public class AutorLibro
    {
        // LLaves primaria compuestas de estas dos para no crear otra solo para esto (PAra esto nos vamos a DBcontext)
        public int LibroID { get; set; }
        public int AutorID { get; set; }
        // Relacion Muchos a muchos
        public Libro Libro { get; set; }
        public Autor Autor { get; set; }
    }
}
