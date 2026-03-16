using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnodicaV2DEMO.ViewModels
{
    public class InsumoMovimientoVM
    {
        [Required(ErrorMessage = "Debe seleccionar un insumo.")]
        public short InsumoRef { get; set; }

        public int? ProveedorRef { get; set; }

        public short? OperarioRetiroRef { get; set; }

        [Required(ErrorMessage = "Debe especificar si es ingreso o egreso.")]
        public bool EsIngreso { get; set; }

        [Required(ErrorMessage = "Debe ingresar la cantidad.")]
        [Range(1, short.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public short Cantidad { get; set; }

        [Required(ErrorMessage = "La fecha del movimiento es obligatoria.")]
        public DateTime FechaMovimiento { get; set; }
        public IEnumerable<SelectListItem>? ListaInsumos { get; set; }
    }
}