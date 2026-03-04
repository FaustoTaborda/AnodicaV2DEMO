using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anodica.Modelos
{
    [Table("TipoProveedor")]
    public class TipoProveedor
    {
        [Key]
        public byte TipoProveedorID { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de proveedor es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El nombre no puede superar los 200 caracteres.")]
        public string TipoProveedorNombre { get; set; }
    }
}