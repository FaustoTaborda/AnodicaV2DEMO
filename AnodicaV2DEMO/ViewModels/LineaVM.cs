using Microsoft.AspNetCore.Mvc.Rendering;

namespace Anodica.Modelos.ViewModels
{
    public class LineaVM
    {
        public Linea Linea { get; set; }
        public IEnumerable<SelectListItem> ProveedoresList { get; set; }
    }
}