using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;
using Anodica.Modelos.ViewModels; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Anodica.Controllers
{
    public class LineasController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ILogger<LineasController> _logger;

        public LineasController(IUnidadTrabajo unidadTrabajo, ILogger<LineasController> logger)
        {
            _unidadTrabajo = unidadTrabajo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lista = await _unidadTrabajo.Linea.ObtenerTodosAsync(incluirPropiedades: "Proveedor");
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            LineaVM lineaVM = new LineaVM()
            {
                Linea = new Linea(),
                ProveedoresList = (await _unidadTrabajo.Proveedor.ObtenerTodosAsync()).Select(p => new SelectListItem
                {
                    Text = p.ProveedorNombre,
                    Value = p.ProveedorID.ToString()
                })
            };

            return View(lineaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LineaVM lineaVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var validaLineaExistente = await _unidadTrabajo.Linea.ObtenerTodosAsync(l => l.LineaNombre.Trim().ToLower() == lineaVM.Linea.LineaNombre.Trim().ToLower());

                    if (validaLineaExistente.Any())
                    {
                        ModelState.AddModelError("Linea.LineaNombre", $"El nombre de línea '{lineaVM.Linea.LineaNombre}' ya existe.");
                        lineaVM.ProveedoresList = (await _unidadTrabajo.Proveedor.ObtenerTodosAsync()).Select(p => new SelectListItem { Text = p.ProveedorNombre, Value = p.ProveedorID.ToString() });
                        return View(lineaVM);
                    }

                    _unidadTrabajo.Linea.Agregar(lineaVM.Linea);
                    await _unidadTrabajo.GuardarAsync();

                    TempData["success"] = "Línea creada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error crítico al intentar crear la línea.");
                    ModelState.AddModelError(string.Empty, "Ocurrió un error interno. Intente nuevamente.");
                }
            }

            lineaVM.ProveedoresList = (await _unidadTrabajo.Proveedor.ObtenerTodosAsync()).Select(p => new SelectListItem { Text = p.ProveedorNombre, Value = p.ProveedorID.ToString() });
            return View(lineaVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null) return NotFound();

            LineaVM lineaVM = new LineaVM()
            {
                Linea = await _unidadTrabajo.Linea.ObtenerAsync(id.Value),
                ProveedoresList = (await _unidadTrabajo.Proveedor.ObtenerTodosAsync()).Select(p => new SelectListItem
                {
                    Text = p.ProveedorNombre,
                    Value = p.ProveedorID.ToString()
                })
            };

            if (lineaVM.Linea == null) return NotFound();

            return View(lineaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LineaVM lineaVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unidadTrabajo.Linea.Actualizar(lineaVM.Linea);
                    await _unidadTrabajo.GuardarAsync();

                    TempData["success"] = "Línea actualizada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al actualizar la línea con ID {Id}", lineaVM.Linea.LineaID);
                    ModelState.AddModelError(string.Empty, "No se pudo actualizar. Verifique los datos e intente nuevamente.");
                }
            }

            lineaVM.ProveedoresList = (await _unidadTrabajo.Proveedor.ObtenerTodosAsync()).Select(p => new SelectListItem { Text = p.ProveedorNombre, Value = p.ProveedorID.ToString() });
            return View(lineaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                var objDesdeDb = await _unidadTrabajo.Linea.ObtenerAsync(id);

                if (objDesdeDb == null)
                {
                    TempData["error"] = "La línea no existe o ya fue eliminada.";
                    return RedirectToAction(nameof(Index));
                }

                _unidadTrabajo.Linea.Remover(objDesdeDb);
                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Línea eliminada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar eliminar la línea con ID {Id}. Posible conflicto de clave foránea.", id);
                TempData["error"] = "No se puede eliminar esta línea porque tiene perfiles asociados en el sistema.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}