using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Anodica.Modelos;
namespace AnodicaV2DEMO.ViewModels
{
    public class PerfilTratamientoFilaVM
    {
        public short TratamientoRef { get; set; }
        public string TratamientoNombre { get; set; }

        public bool EstaSeleccionado { get; set; }

        public short? UbicacionRef { get; set; }
        public short CantMinimaTirasStock { get; set; }
    }
}
