using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anodica.Modelos
{
    [Table("Insumo")]
    public class Insumo
    {
        [Key]
        public short InsumoID { get; set; } 

        [Required(ErrorMessage = "El código es obligatorio")]
        [Column(TypeName = "varchar(50)")] 
        public string CodigoInsumo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Column(TypeName = "varchar(500)")]
        public string InsumoNombre { get; set; }

        [Required(ErrorMessage = "La unidad es obligatoria")]
        [Column(TypeName = "varchar(5)")]
        public string UnidadMedida { get; set; }
        [Range(0, 100000, ErrorMessage = "El stock no puede ser un número negativo.")] 
        [Required(ErrorMessage = "El stock es obligatorio")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal CantidadStock { get; set; } 
        [Range(0, 100000, ErrorMessage = "El stock mínimo no puede ser un número negativo.")]
        [Required(ErrorMessage = "El stock mínimo es obligatorio")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal CantMinimaStock { get; set; } 

        public ICollection<InsumoMovimiento> InsumoMovimientos { get; set; }
    }
}