using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Anodica.Modelos
{
    [Table("InsumoMovimiento")]
    public class InsumoMovimiento
    {
        [Key]
        public int InsumoMovimientoID { get; set;}

        //DATOS RELACIONADOS A INSUMO
        [Required]
        public short InsumoRef { get; set;}
        [ForeignKey("InsumoRef")]
        public Insumo Insumo { get; set; }

        public int? ProveedorRef { get; set; }
        public short? OperarioRetiroRef { get; set; }
        [Required]
        public bool EsIngreso { get; set; }
        [Required]
        public short Cantidad { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime FechaMovimiento { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime FechaCreacion { get; set; }
        [Required]
        public Guid UserAccountRef { get; set; }

    }
}
