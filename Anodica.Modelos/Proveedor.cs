using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anodica.Modelos
{
    [Table("Proveedor")]
    public class Proveedor
    {
        [Key]
        public int ProveedorID { get; set; }
        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        [MaxLength(300, ErrorMessage = "El nombre del proveedor no puede superar los 300 caracteres.")]
        public string ProveedorNombre { get; set; }

        [MaxLength(300, ErrorMessage = "Los teléfonos no pueden superar los 300 caracteres.")]
        public string? Telefonos { get; set; }

        [MaxLength(100, ErrorMessage = "El email no puede superar los 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string? Email { get; set; }

        [MaxLength(300, ErrorMessage = "La lista de productos no puede superar los 300 caracteres.")]
        public string? Productos { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal? PorcentajePesoTiraPerfil { get; set; }
    }
}