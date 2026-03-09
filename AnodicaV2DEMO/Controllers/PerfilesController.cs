using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;
using AnodicaV2DEMO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Anodica.Web.Controllers
{
    public class PerfilesController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ILogger<PerfilesController> _logger;

        public PerfilesController(IUnidadTrabajo unidadTrabajo, ILogger<PerfilesController> logger)
        {
            _unidadTrabajo = unidadTrabajo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var perfiles = await _unidadTrabajo.Perfil.ObtenerTodosAsync(incluirPropiedades: "Linea,Linea.Proveedor,Ubicacion");
            return View(perfiles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            PerfilVM perfilVM = new PerfilVM()
            {
                Perfil = new Perfil()
            };

            await CargarListasDelViewModel(perfilVM);

            return View(perfilVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerfilVM perfilVM)
        {
            if (!ModelState.IsValid)
            {
                await CargarListasDelViewModel(perfilVM);
                return View(perfilVM);
            }

            perfilVM.Perfil.PerfilCodigoAlcemar = perfilVM.Perfil.PerfilCodigoAlcemar?.Trim();
            perfilVM.Perfil.Descripcion = perfilVM.Perfil.Descripcion?.Trim();
            perfilVM.Perfil.PesoXtira = perfilVM.Perfil.PesoXmetro * perfilVM.Perfil.LongTiraMts;

            try
            {
                var existeCodigo = await _unidadTrabajo.Perfil.ObtenerTodosAsync(p => p.PerfilCodigoAlcemar == perfilVM.Perfil.PerfilCodigoAlcemar);
                if (existeCodigo.Any())
                {
                    ModelState.AddModelError("Perfil.PerfilCodigoAlcemar", "Ya existe un perfil con este código Alcemar.");
                    await CargarListasDelViewModel(perfilVM);
                    return View(perfilVM);
                }

                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count > 0)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await archivos[0].CopyToAsync(dataStream);
                        perfilVM.Perfil.ImagenPerfil = dataStream.ToArray();
                    }
                }

                _unidadTrabajo.Perfil.Agregar(perfilVM.Perfil);
                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Perfil industrial creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear perfil Alcemar.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error interno al guardar.");
                await CargarListasDelViewModel(perfilVM);
                return View(perfilVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var perfilOriginal = (await _unidadTrabajo.Perfil.ObtenerTodosAsync(
                filtro: p => p.PerfilID == id.Value,
                incluirPropiedades: "Linea"
            )).FirstOrDefault();

            if (perfilOriginal == null) return NotFound();

            PerfilVM perfilVM = new PerfilVM()
            {
                Perfil = perfilOriginal,
                ProveedorId = perfilOriginal.Linea?.ProveedorRef
            };

            await CargarListasDelViewModel(perfilVM);

            return View(perfilVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PerfilVM perfilVM)
        {
            if (!ModelState.IsValid)
            {
                await CargarListasDelViewModel(perfilVM);
                return View(perfilVM);
            }

            perfilVM.Perfil.PerfilCodigoAlcemar = perfilVM.Perfil.PerfilCodigoAlcemar?.Trim();
            perfilVM.Perfil.Descripcion = perfilVM.Perfil.Descripcion?.Trim();
            perfilVM.Perfil.PesoXtira = perfilVM.Perfil.PesoXmetro * perfilVM.Perfil.LongTiraMts;

            try
            {
                var existeCodigo = await _unidadTrabajo.Perfil.ObtenerTodosAsync(p =>
                    p.PerfilCodigoAlcemar == perfilVM.Perfil.PerfilCodigoAlcemar &&
                    p.PerfilID != perfilVM.Perfil.PerfilID);

                if (existeCodigo.Any())
                {
                    ModelState.AddModelError("Perfil.PerfilCodigoAlcemar", "El código ya está siendo usado por otro perfil.");
                    await CargarListasDelViewModel(perfilVM);
                    return View(perfilVM);
                }

                var perfilOriginal = (await _unidadTrabajo.Perfil.ObtenerTodosAsync(
                    filtro: p => p.PerfilID == perfilVM.Perfil.PerfilID,
                    isTracking: false
                )).FirstOrDefault();

                var archivos = HttpContext.Request.Form.Files;

                if (archivos.Count > 0)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await archivos[0].CopyToAsync(dataStream);
                        perfilVM.Perfil.ImagenPerfil = dataStream.ToArray();
                    }
                }
                else
                {
                    perfilVM.Perfil.ImagenPerfil = perfilOriginal.ImagenPerfil;
                }

                _unidadTrabajo.Perfil.Actualizar(perfilVM.Perfil);
                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Perfil actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar perfil ID {Id}", perfilVM.Perfil.PerfilID);
                ModelState.AddModelError(string.Empty, "No se pudieron guardar los cambios.");
                await CargarListasDelViewModel(perfilVM);
                return View(perfilVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var perfil = await _unidadTrabajo.Perfil.ObtenerAsync(id);
                if (perfil == null)
                {
                    TempData["error"] = "El perfil no existe.";
                    return RedirectToAction(nameof(Index));
                }

                _unidadTrabajo.Perfil.Remover(perfil);
                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Perfil industrial eliminado.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de clave foránea al borrar perfil {Id}", id);
                TempData["error"] = "No se puede eliminar porque este perfil tiene dependencias.";
            }

            return RedirectToAction(nameof(Index));
        }


        // MÉTODOS AUXILIARES 


        [HttpGet]
        public async Task<IActionResult> ObtenerLineasPorProveedor(int proveedorId)
        {
            var lineas = await _unidadTrabajo.Linea.ObtenerTodosAsync(l => l.ProveedorRef == proveedorId);

            return Json(lineas.Select(l => new {
                value = l.LineaID,
                text = l.LineaNombre
            }));
        }

        private async Task CargarListasDelViewModel(PerfilVM vm)
        { 
            var lineas = await _unidadTrabajo.Linea.ObtenerTodosAsync();
            var idsProveedoresConLineas = lineas.Select(l => l.ProveedorRef).Distinct().ToList();


            var proveedoresFiltrados = await _unidadTrabajo.Proveedor.ObtenerTodosAsync(
                filtro: p => idsProveedoresConLineas.Contains(p.ProveedorID)
            );

            vm.ProveedoresList = proveedoresFiltrados.Select(p => new SelectListItem
            {
                Text = p.ProveedorNombre,
                Value = p.ProveedorID.ToString()
            });

            vm.UbicacionesList = (await _unidadTrabajo.Ubicacion.ObtenerTodosAsync()).Select(u => new SelectListItem
            {
                Text = u.UbicacionCodigo == u.UbicacionDesc ? u.UbicacionCodigo : $"{u.UbicacionCodigo} - {u.UbicacionDesc}",
                Value = u.UbicacionID.ToString()
            });

            if (vm.ProveedorId.HasValue && vm.ProveedorId.Value > 0)
            {
                var lineasDelProveedor = await _unidadTrabajo.Linea.ObtenerTodosAsync(l => l.ProveedorRef == vm.ProveedorId.Value);
                vm.LineasList = lineasDelProveedor.Select(l => new SelectListItem
                {
                    Text = l.LineaNombre,
                    Value = l.LineaID.ToString()
                });
            }
            else
            {
                vm.LineasList = new List<SelectListItem>();
            }
        }
    }
}