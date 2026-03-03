using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Anodica.Modelos;
namespace AnodicaV2DEMO.ViewModels
{
    public class InsumoMovimientoVM
    {
        public InsumoMovimiento InsumoMovimiento { get; set; }
        public IEnumerable<SelectListItem> ListaInsumos { get; set; }
    }
}
