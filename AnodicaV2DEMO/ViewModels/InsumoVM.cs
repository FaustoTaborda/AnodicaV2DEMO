using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // Por si querés hacer la unidad de medida un Dropdown

namespace AnodicaV2DEMO.ViewModels
{
    public class InsumoVM
    {
        public short InsumoID { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [MaxLength(50, ErrorMessage = "El código no puede superar los 50 caracteres.")]
        public string CodigoInsumo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(500, ErrorMessage = "El nombre no puede superar los 500 caracteres.")]
        public string InsumoNombre { get; set; }

        [Required(ErrorMessage = "La unidad es obligatoria")]
        [MaxLength(5, ErrorMessage = "La unidad de medida es muy larga.")]
        public string UnidadMedida { get; set; }

        public IEnumerable<SelectListItem>? UnidadesList { get; set; }

        [Required(ErrorMessage = "El stock mínimo es obligatorio")]
        [Range(0, 100000, ErrorMessage = "El stock mínimo no puede ser un número negativo.")]
        public decimal CantMinimaStock { get; set; }
    }
}