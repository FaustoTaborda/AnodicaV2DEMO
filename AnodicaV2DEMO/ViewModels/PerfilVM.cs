using Anodica.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnodicaV2DEMO.ViewModels
{
    public class PerfilVM
    {
        public Perfil Perfil { get; set; }

        public int? ProveedorId { get; set; }
        public IEnumerable<SelectListItem> ProveedoresList { get; set; }
        public IEnumerable<SelectListItem> LineasList { get; set; }
        public IEnumerable<SelectListItem> UbicacionesList { get; set; }
    }
}