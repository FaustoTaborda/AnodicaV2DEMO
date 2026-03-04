using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anodica.Modelos
{
    [Table("Tratamiento")]
    public class Tratamiento
    {
        [Key]
        public short TratamientoID { get; set; }

        [Required(ErrorMessage = "El nombre del tratamiento es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string TratamientoNombre { get; set; }

        [Column(TypeName = "char(7)")]//Codigo HEXA
        [StringLength(7)]
        public string? TratamientoColorFondo { get; set; }

        [Column(TypeName = "char(7)")]//Codigo HEXA
        [StringLength(7)]
        public string? TratamientoColorFuente { get; set; }

        [Column(TypeName = "money")]
        public decimal? PrecioXKgTratamiento { get; set; }

        public byte? Orden { get; set; }
    }
}