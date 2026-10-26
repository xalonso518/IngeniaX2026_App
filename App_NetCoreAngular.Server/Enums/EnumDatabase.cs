using System.ComponentModel;

namespace App_NetCoreAngular.Server.Enums
{
    public class EnumDatabase
    {
        public enum Estado
        {
            Activo = 1,
            Inactivo = 2,
            Eliminado = 4,
            Baja = 99,
        }

        public enum RolUsuario
        {
            Admin = 1,
            Cliente = 2,
        }
    }
}
