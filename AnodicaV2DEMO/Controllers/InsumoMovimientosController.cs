using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;
using AnodicaV2DEMO.ViewModels;
using MapsterMapper; // ¡Agregado Mapster!
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anodica.Controllers
{
    public class InsumoMovimientosController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ILogger<InsumoMovimientosController> _logger;
        private readonly IMapper _mapper; 

        public InsumoMovimientosController(IUnidadTrabajo unidadTrabajo, ILogger<InsumoMovimientosController> logger, IMapper mapper)
        {
            _unidadTrabajo = unidadTrabajo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listaMovimientos = await _unidadTrabajo.InsumoMovimiento.ObtenerTodosAsync(incluirPropiedades: "Insumo");
            return View(listaMovimientos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            InsumoMovimientoVM movimientoVM = new InsumoMovimientoVM()
            {
                FechaMovimiento = DateTime.Now,
                ListaInsumos = await ObtenerListaInsumosParaDropdown()
            };

            return View(movimientoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsumoMovimientoVM movimientoVM)
        {
            if (!ModelState.IsValid)
            {
                movimientoVM.ListaInsumos = await ObtenerListaInsumosParaDropdown();
                return View(movimientoVM);
            }

            try
            {
                var insumoDb = await _unidadTrabajo.Insumo.ObtenerAsync(movimientoVM.InsumoRef);

                if (insumoDb == null)
                {
                    TempData["error"] = "El insumo seleccionado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                if (movimientoVM.EsIngreso)
                {
                    insumoDb.CantidadStock += movimientoVM.Cantidad;
                }
                else
                {
                    if (insumoDb.CantidadStock - movimientoVM.Cantidad < insumoDb.CantMinimaStock)
                    {
                        ModelState.AddModelError("Cantidad", $"No hay stock suficiente. Stock actual: {insumoDb.CantidadStock}, Mínimo permitido: {insumoDb.CantMinimaStock}");
                        movimientoVM.ListaInsumos = await ObtenerListaInsumosParaDropdown();
                        return View(movimientoVM);
                    }
                    insumoDb.CantidadStock -= movimientoVM.Cantidad;
                }
                InsumoMovimiento nuevoMovimiento = _mapper.Map<InsumoMovimiento>(movimientoVM);
                nuevoMovimiento.FechaCreacion = DateTime.Now;
                nuevoMovimiento.UserAccountRef = Guid.NewGuid();

                _unidadTrabajo.InsumoMovimiento.Agregar(nuevoMovimiento);
                
                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Movimiento registrado y stock actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al registrar el movimiento del Insumo ID: {InsumoRef}", movimientoVM.InsumoRef);
                ModelState.AddModelError(string.Empty, "Ocurrió un error interno al guardar el movimiento. Intente nuevamente.");
                movimientoVM.ListaInsumos = await ObtenerListaInsumosParaDropdown();
                return View(movimientoVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerStockInsumo(short id)
        {
            var insumo = await _unidadTrabajo.Insumo.ObtenerAsync(id);
            if (insumo == null)
            {
                return Json(new { success = false });
            }
            
            string nombreUnidad = insumo.UnidadMedida switch
            {
                "Un" => "Unidades",
                "Kg" => "Kilogramos",
                "Lt" => "Litros",
                _ => insumo.UnidadMedida 
            };

            return Json(new
            {
                success = true,
                stockActual = insumo.CantidadStock,
                stockMinimo = insumo.CantMinimaStock,
                unidad = nombreUnidad
            });
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerListaInsumosParaDropdown()
        {
            var insumosDesdeBd = await _unidadTrabajo.Insumo.ObtenerTodosAsync(isTracking: false);
            return insumosDesdeBd.Select(i => new SelectListItem
            {
                Text = i.InsumoNombre,
                Value = i.InsumoID.ToString()
            });
        }
    }
}