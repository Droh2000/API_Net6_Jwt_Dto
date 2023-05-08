namespace WebApiAutoresCLase.DTOs
{
    //Aqui esta heredando
    public class AutorDTOConLibros: AutorDto
    {
        //Saber la lista de todos librros que tiene el autor
        public List<LibroDTO> Libros { get; set; }
    }
}
