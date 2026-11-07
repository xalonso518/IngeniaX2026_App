using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static App_NetCoreAngular.Server.Enums.EnumDatabase;

namespace App_NetCoreAngular.Server.Models.Productos
{
    public class ProductoSimpleView
    {
        public long ProductoId { get; set; }
        [StringLength(250)]
        public required string Nombre { get; set; }
        [StringLength(50)]
        public required string Codigo { get; set; }
        [StringLength(250)]
        public string? Imagen { get; set; }
        [Required]
        [Precision(12, 2)]
        public required decimal Precio { get; set; }
        [Required]
        [Precision(12, 2)]
        public required decimal Existencia { get; set; }
        [Required]
        public Estado Estado { get; set; }
    }
}
