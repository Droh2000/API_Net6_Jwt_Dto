using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiAutoresCLase.Models;

namespace WebApiAutoresCLase.Data
{
    // Ahora hereda de esto con Identity (Para los DTOS)
    public class AplicationDbContext:IdentityDbContext
    {
        public AplicationDbContext(DbContextOptions options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // por el identyty se pone
            base.OnModelCreating(modelBuilder); //SE sobreescribe pero corre todo el modelo y depues hace todo lo que ponemos

            // Lave Primaria Compuesta (Relacion Muchos a muchos)
            modelBuilder.Entity<AutorLibro>()
                .HasKey(x => new { x.AutorID, x.LibroID });
        }

        // al Hacer a migracion se crearon muchas tablas en la BD de Users y Tokens
        // tokens guarda el id y el provedor entre otros, al login se crea un registro con la fecha de login y expiracion
        // al hacer una trnsaccion al servidor se puede hacer que no expire
        // roles son claims no es crea la autenticacion de usuario con toda la seguridad


        // SE agraga la relacion
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<AutorLibro> AutoresLibros { get; set; }
        public DbSet<ApplicationUser> Usuarios { get; set; }

    }
}
