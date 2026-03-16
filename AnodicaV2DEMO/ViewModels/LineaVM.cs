using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Anodica.Modelos.ViewModels
{
    public class LineaVM
    {
        public short LineaID { get; set; }

        [Required(ErrorMessage = "El nombre de la línea es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string LineaNombre { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        public int ProveedorRef { get; set; }

        public byte? LineaGrupoRef { get; set; }

        public IEnumerable<SelectListItem>? ProveedoresList { get; set; }
    }
}