using System.ComponentModel.DataAnnotations;

namespace App_NetCoreAngular.Server.Models.Clientes
{
    public class ClienteForm
    {
        [Required]
        [StringLength(250)]
        public required string Nombre { get; set; }

        [Required]
        [StringLength(500)]
        public required string Descripcion { get; set; }
    }
}
