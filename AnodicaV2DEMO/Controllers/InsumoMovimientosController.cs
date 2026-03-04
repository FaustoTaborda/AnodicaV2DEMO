using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;
using AnodicaV2DEMO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Anodica.Controllers
{
    public class InsumoMovimientosController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ILogger<InsumoMovimientosController> _logger;


        public InsumoMovimientosController(IUnidadTrabajo unidadTrabajo, ILogger<InsumoMovimientosController> logger)
        {
            _unidadTrabajo = unidadTrabajo;
            _logger = logger;
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
                InsumoMovimiento = new InsumoMovimiento(),
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
                // Los dropdown necesitan ser recargados si el modelo no es válido para que la vista pueda renderizar correctamente
                movimientoVM.ListaInsumos = await ObtenerListaInsumosParaDropdown();
                return View(movimientoVM);
            }

            try
            {
                var insumoDb = await _unidadTrabajo.Insumo.ObtenerAsync(movimientoVM.InsumoMovimiento.InsumoRef);

                if (insumoDb == null)
                {
                    TempData["error"] = "El insumo seleccionado no existe.";
                    return RedirectToAction(nameof(Index));
                }
                if (movimientoVM.InsumoMovimiento.EsIngreso)
                {
                    insumoDb.CantidadStock += movimientoVM.InsumoMovimiento.Cantidad;
                }
                else
                {
                    // Validamos el Stock Mínimo
                    if (insumoDb.CantidadStock - movimientoVM.InsumoMovimiento.Cantidad < insumoDb.CantMinimaStock)
                    {
                        ModelState.AddModelError("InsumoMovimiento.Cantidad", $"No hay stock suficiente. Stock actual: {insumoDb.CantidadStock}, Mínimo permitido: {insumoDb.CantMinimaStock}");
                        movimientoVM.ListaInsumos = await ObtenerListaInsumosParaDropdown();
                        return View(movimientoVM);
                    }
                    insumoDb.CantidadStock -= movimientoVM.InsumoMovimiento.Cantidad;
                }

                movimientoVM.InsumoMovimiento.FechaCreacion = DateTime.Now;
                // NOTA // Asignamos un Guid vacío momentaneamente
                movimientoVM.InsumoMovimiento.UserAccountRef = Guid.NewGuid();
                _unidadTrabajo.InsumoMovimiento.Agregar(movimientoVM.InsumoMovimiento);
                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Movimiento registrado y stock actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al registrar el movimiento del Insumo ID: {InsumoRef}", movimientoVM.InsumoMovimiento.InsumoRef);
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
            //Para que la vista muestre el nombre completo de la unidad de medida en lugar de abreviada
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

        // Metodo Auxiliar para cargar el dropdown de Insumos en la vista Create y Edit
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