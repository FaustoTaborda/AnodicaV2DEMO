using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;
using Anodica.Modelos.ViewModels; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MapsterMapper; 

namespace Anodica.Controllers
{
    public class LineasController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ILogger<LineasController> _logger;
        private readonly IMapper _mapper; 
        public LineasController(IUnidadTrabajo unidadTrabajo, ILogger<LineasController> logger, IMapper mapper)
        {
            _unidadTrabajo = unidadTrabajo;
            _logger = logger;
            _mapper = mapper;
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
                    var validaLineaExistente = await _unidadTrabajo.Linea.ObtenerTodosAsync(l => l.LineaNombre.Trim().ToLower() == lineaVM.LineaNombre.Trim().ToLower());

                    if (validaLineaExistente.Any())
                    {
                        ModelState.AddModelError("LineaNombre", $"El nombre de línea '{lineaVM.LineaNombre}' ya existe.");
                        lineaVM.ProveedoresList = (await _unidadTrabajo.Proveedor.ObtenerTodosAsync()).Select(p => new SelectListItem { Text = p.ProveedorNombre, Value = p.ProveedorID.ToString() });
                        return View(lineaVM);
                    }

                    Linea nuevaLinea = _mapper.Map<Linea>(lineaVM);
                    _unidadTrabajo.Linea.Agregar(nuevaLinea);
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

            var lineaOriginal = await _unidadTrabajo.Linea.ObtenerAsync(id.Value);
            if (lineaOriginal == null) return NotFound();
            LineaVM lineaVM = _mapper.Map<LineaVM>(lineaOriginal);
            
            lineaVM.ProveedoresList = (await _unidadTrabajo.Proveedor.ObtenerTodosAsync()).Select(p => new SelectListItem
            {
                Text = p.ProveedorNombre,
                Value = p.ProveedorID.ToString()
            });

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
                    var validaLineaExistente = await _unidadTrabajo.Linea.ObtenerTodosAsync(l => 
                        l.LineaNombre.Trim().ToLower() == lineaVM.LineaNombre.Trim().ToLower() && 
                        l.LineaID != lineaVM.LineaID);

                    if (validaLineaExistente.Any())
                    {
                        ModelState.AddModelError("LineaNombre", $"El nombre de línea '{lineaVM.LineaNombre}' ya está siendo usado.");
                        lineaVM.ProveedoresList = (await _unidadTrabajo.Proveedor.ObtenerTodosAsync()).Select(p => new SelectListItem { Text = p.ProveedorNombre, Value = p.ProveedorID.ToString() });
                        return View(lineaVM);
                    }

                    var lineaDesdeBd = await _unidadTrabajo.Linea.ObtenerAsync(lineaVM.LineaID);
                    if (lineaDesdeBd == null) return NotFound();
                    _mapper.Map(lineaVM, lineaDesdeBd);
                    _unidadTrabajo.Linea.Actualizar(lineaDesdeBd);
                    await _unidadTrabajo.GuardarAsync();

                    TempData["success"] = "Línea actualizada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al actualizar la línea con ID {Id}", lineaVM.LineaID);
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