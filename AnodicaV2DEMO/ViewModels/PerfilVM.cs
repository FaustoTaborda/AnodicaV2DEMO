using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnodicaV2DEMO.ViewModels
{
    public class PerfilVM
    {
        public int PerfilID { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        [MaxLength(50)]
        public string PerfilCodigoAlcemar { get; set; }

        [Required(ErrorMessage = "La línea es obligatoria.")]
        public short LineaRef { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [MaxLength(500)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El peso es obligatorio.")]
        public decimal PesoXmetro { get; set; }

        [Required(ErrorMessage = "La longitud es obligatoria.")]
        public decimal LongTiraMts { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        public byte CantTirasPaquete { get; set; }

        public bool ManejaStockPropio { get; set; }
        public byte[]? ImagenPerfil { get; set; }

        public int? ProveedorId { get; set; }
        public IEnumerable<SelectListItem>? ProveedoresList { get; set; }
        public IEnumerable<SelectListItem>? LineasList { get; set; }
        public IEnumerable<SelectListItem>? UbicacionesList { get; set; }
        public List<PerfilTratamientoFilaVM> Tratamientos { get; set; } = new List<PerfilTratamientoFilaVM>();
    }
}