using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Anodica.Controllers
{
    public class InsumosController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ILogger<InsumosController> _logger;

        // Inyectamos la Unidad de Trabajo y el Logger para registrar errores 
        public InsumosController(IUnidadTrabajo unidadTrabajo, ILogger<InsumosController> logger)
        {
            _unidadTrabajo = unidadTrabajo;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lista = await _unidadTrabajo.Insumo.ObtenerTodosAsync();
            return View(lista);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Insumo insumo)
        {
            if (!ModelState.IsValid)
            {
                return View(insumo);
            }

            try
            {
                _unidadTrabajo.Insumo.Agregar(insumo);
                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Insumo creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // El error detallado va al archivo de log interno
                _logger.LogError(ex, "Error crítico al intentar crear el insumo.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error interno al guardar el insumo. Intente nuevamente.");
                return View(insumo);
            }
        }

        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null) return NotFound();

            var insumo = await _unidadTrabajo.Insumo.ObtenerAsync(id.Value);

            if (insumo == null) return NotFound();

            return View(insumo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Insumo insumo)
        {
            if (!ModelState.IsValid)
            {
                return View(insumo);
            }

            try
            {
                _unidadTrabajo.Insumo.Actualizar(insumo);
                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Insumo actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el insumo con ID {Id}", insumo.InsumoID);
                ModelState.AddModelError(string.Empty, "No se pudo actualizar el insumo. Verifique los datos e intente nuevamente.");
                return View(insumo);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                var objDesdeDb = await _unidadTrabajo.Insumo.ObtenerAsync(id);
                if (objDesdeDb == null)
                {
                    TempData["error"] = "El insumo no existe o ya fue eliminado.";
                    return RedirectToAction(nameof(Index));
                }

                _unidadTrabajo.Insumo.Remover(objDesdeDb);
                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Insumo eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar eliminar el insumo con ID {Id}. Posible conflicto de clave foránea.", id);
                TempData["error"] = "No se puede eliminar este insumo porque tiene registros asociados en el sistema.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}