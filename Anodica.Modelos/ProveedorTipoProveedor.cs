using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anodica.Modelos
{
    [Table("ProveedorTipoProveedor")]
    public class ProveedorTipoProveedor
    {
        [Key]
        public int ProveedorTipoProveedorID { get; set; }

        [Required]
        public int ProveedorRef { get; set; }

        [Required]
        public byte TipoProveedorRef { get; set; }


        [ForeignKey("ProveedorRef")]
        public Proveedor Proveedor { get; set; }

        [ForeignKey("TipoProveedorRef")]
        public TipoProveedor TipoProveedor { get; set; }
    }
}