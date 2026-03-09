using Anodica.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace AnodicaV2DEMO.ViewModels
{
    public class PerfilTratamientoVM
    {
        public PerfilTratamiento PerfilTratamiento { get; set; }
        public IEnumerable<SelectListItem> PerfilesList { get; set; }
        public IEnumerable<SelectListItem> TratamientosList { get; set; }
        public IEnumerable<SelectListItem> UbicacionesList { get; set; }
    }
}