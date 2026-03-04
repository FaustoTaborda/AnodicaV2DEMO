using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anodica.Modelos
{
    [Table("Linea")]
    public class Linea
    {
        [Key]
        public short LineaID { get; set; }

        [Required(ErrorMessage = "El nombre de la línea es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string LineaNombre { get; set; }

        [Required]
        public int ProveedorRef { get; set; }

        public byte? LineaGrupoRef { get; set; }

        [ForeignKey("ProveedorRef")]
        public Proveedor Proveedor { get; set; }
    }
}