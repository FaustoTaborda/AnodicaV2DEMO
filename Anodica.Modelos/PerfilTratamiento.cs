using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anodica.Modelos
{
    [Table("PerfilTratamiento")]
    public class PerfilTratamiento
    {
        [Key]
        public int PerfilTratamientoId { get; set; }

        [Required]
        public int PerfilRef { get; set; }

        [Required]
        public short TratamientoRef { get; set; }

        [Required]
        public short CantMinimaTirasStock { get; set; }

        [Required]
        public short CantidadStock { get; set; }
        public short? UbicacionRef { get; set; }

        [ForeignKey("PerfilRef")]
        public Perfil Perfil { get; set; }

        [ForeignKey("TratamientoRef")]
        public Tratamiento Tratamiento { get; set; }

        [ForeignKey("UbicacionRef")]
        public Ubicacion Ubicacion { get; set; }
    }
}