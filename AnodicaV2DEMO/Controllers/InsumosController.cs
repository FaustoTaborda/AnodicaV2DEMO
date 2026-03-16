using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;
using AnodicaV2DEMO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MapsterMapper; 

namespace Anodica.Controllers
{
    public class InsumosController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ILogger<InsumosController> _logger;
        private readonly IMapper _mapper; 

        public InsumosController(IUnidadTrabajo unidadTrabajo, ILogger<InsumosController> logger, IMapper mapper)
        {
            _unidadTrabajo = unidadTrabajo;
            _logger = logger;
            _mapper = mapper;
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
            return View(new InsumoVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsumoVM insumoVM)
        {
            if (!ModelState.IsValid)
            {
                return View(insumoVM);
            }

            try
            {
                var validaInsumoExistente = await _unidadTrabajo.Insumo.ObtenerTodosAsync(i => i.CodigoInsumo == insumoVM.CodigoInsumo);

                if (validaInsumoExistente.Any())
                {
                    ModelState.AddModelError("CodigoInsumo", $"El código '{insumoVM.CodigoInsumo}' ya está en uso por otro insumo.");
                    return View(insumoVM);
                }

                // Conversion VM -> DB
                Insumo insumoParaBD = _mapper.Map<Insumo>(insumoVM);

                _unidadTrabajo.Insumo.Agregar(insumoParaBD);
                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Insumo creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al intentar crear el insumo.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error interno al guardar el insumo. Intente nuevamente.");
                return View(insumoVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null) return NotFound();

            var insumo = await _unidadTrabajo.Insumo.ObtenerAsync(id.Value);

            if (insumo == null) return NotFound();

            // Conversion de DB -> VM
            var insumoVM = _mapper.Map<InsumoVM>(insumo);

            return View(insumoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InsumoVM insumoVM)
        {
            if (!ModelState.IsValid)
            {
                return View(insumoVM);
            }

            try
            {
                var validaInsumoExistente = await _unidadTrabajo.Insumo.ObtenerTodosAsync(i => i.CodigoInsumo == insumoVM.CodigoInsumo && i.InsumoID != insumoVM.InsumoID);
                if (validaInsumoExistente.Any())
                {
                    ModelState.AddModelError("CodigoInsumo", $"El código '{insumoVM.CodigoInsumo}' ya está en uso por otro insumo.");
                    return View(insumoVM);
                }

                var insumoDesdeBd = await _unidadTrabajo.Insumo.ObtenerAsync(insumoVM.InsumoID);
                if (insumoDesdeBd == null) return NotFound();
                _mapper.Map(insumoVM, insumoDesdeBd);

                _unidadTrabajo.Insumo.Actualizar(insumoDesdeBd);
                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Insumo actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el insumo con ID {Id}", insumoVM.InsumoID);
                ModelState.AddModelError(string.Empty, "No se pudo actualizar el insumo. Verifique los datos e intente nuevamente.");
                return View(insumoVM);
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