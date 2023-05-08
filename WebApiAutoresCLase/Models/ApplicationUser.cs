using Microsoft.AspNetCore.Identity;

namespace WebApiAutoresCLase.Models
{
    // Esto se agrega al Principio
    //esta sea una moidicacion de la tabla original
    // para agregar mas campas a la tabla
    public class ApplicationUser:IdentityUser
    {
        //Aqui agregamos informacion (No se le pone Id porque ya esta heredando demla tabla orignla)
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public int numeroExterior { get; set; }
        public int numeroInteriro { get; set; }

        // antes de la relacion se modifica se agrega dbcontex
        // se modifioca progrma y se modfica en la configuracion de Identityty y se pone este modelo
        // Al hacer migracion en la tabla AspNetUser treae estos campos

        //En todos los controladores que usemos Identityuser se cambia aplicationUser (Todos los campos)
        // Se crea Dto's (Agregar Usuarios) y Credenciales Usuario sea solo para Login
        // se hace el mapeo
        // En cuentas controller se modifica (Registrar -> de credencialesUsuario es ahora este nuevo Dto)
        // SE crea la variable para maapear al modelo

        // se rleacion con autores para saber que usaurio agrega con autores

    }
}
