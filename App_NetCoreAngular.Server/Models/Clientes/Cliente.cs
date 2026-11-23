using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Models.Clientes
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        [Required]
        public long ClienteId { get; set; }

        [Required]
        [StringLength(250)]
        public required string Nombre { get; set; }

        [Required]
        [StringLength(500)]
        public required string Descripcion { get; set; }

        [Required]
        public DateTime FechaAlta { get; set; }

        [Required]
        public Estado Estado { get; set; }
    }
}
