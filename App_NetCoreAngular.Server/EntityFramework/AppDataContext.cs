using App_NetCoreAngular.Server.Models.Usuarios;
using Microsoft.EntityFrameworkCore;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.EntityFramework
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // SEED
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario() { UsuarioId = 1, Username = "admin@ingeniax.mx", Password = "8251F956D4727352AEC6AC2CDEC839D9", Nombre = "Usuario admin", 
                    Rol = RolUsuario.Admin, DateCreated = new DateTime(), Guid = "g00", Estado = Estado.Activo }
            );
        }

    }
}
