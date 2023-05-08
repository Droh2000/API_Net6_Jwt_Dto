namespace WebApiAutoresCLase.DTOs
{
    public class LibroDTOConAutores:LibroDTO
    {
        // El libro puede tener varios autores, la difrencia aqui es esta
        public List<AutorDto> Autores { get; set; }
    }
}
