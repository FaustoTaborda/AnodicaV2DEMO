using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;
using AnodicaV2DEMO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MapsterMapper;

namespace Anodica.Web.Controllers
{
    public class PerfilesController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ILogger<PerfilesController> _logger;
        private readonly IMapper _mapper; 

        public PerfilesController(IUnidadTrabajo unidadTrabajo, ILogger<PerfilesController> logger, IMapper mapper)
        {
            _unidadTrabajo = unidadTrabajo;
            _logger = logger;
            _mapper = mapper; 
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
            PerfilVM perfilVM = new PerfilVM();
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

            perfilVM.PerfilCodigoAlcemar = perfilVM.PerfilCodigoAlcemar?.Trim();
            perfilVM.Descripcion = perfilVM.Descripcion?.Trim();

            try
            {
                var existeCodigo = await _unidadTrabajo.Perfil.ObtenerTodosAsync(p => p.PerfilCodigoAlcemar == perfilVM.PerfilCodigoAlcemar);
                if (existeCodigo.Any())
                {
                    ModelState.AddModelError("PerfilCodigoAlcemar", "Ya existe un perfil con este código.");
                    await CargarListasDelViewModel(perfilVM);
                    return View(perfilVM);
                }

                Perfil perfilParaBD = _mapper.Map<Perfil>(perfilVM);

                perfilParaBD.PesoXtira = perfilParaBD.PesoXmetro * perfilParaBD.LongTiraMts;
                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count > 0)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await archivos[0].CopyToAsync(dataStream);
                        perfilParaBD.ImagenPerfil = dataStream.ToArray();
                    }
                }

                _unidadTrabajo.Perfil.Agregar(perfilParaBD);

                if (perfilVM.Tratamientos != null && perfilVM.Tratamientos.Any(t => t.EstaSeleccionado))
                {
                    var tratamientosActivos = perfilVM.Tratamientos.Where(t => t.EstaSeleccionado).ToList();

                    foreach (var item in tratamientosActivos)
                    {
                        var nuevoPerfilTratamiento = new PerfilTratamiento
                        {
                            Perfil = perfilParaBD, 
                            TratamientoRef = item.TratamientoRef,
                            UbicacionRef = item.UbicacionRef,
                            CantMinimaTirasStock = item.CantMinimaTirasStock,
                            CantidadStock = 0
                        };
                        _unidadTrabajo.PerfilTratamiento.Agregar(nuevoPerfilTratamiento);
                    }
                }

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

            PerfilVM perfilVM = _mapper.Map<PerfilVM>(perfilOriginal);
            perfilVM.ProveedorId = perfilOriginal.Linea?.ProveedorRef;

            var tratamientosGuardados = await _unidadTrabajo.PerfilTratamiento.ObtenerTodosAsync(pt => pt.PerfilRef == id.Value);
            var todosLosTratamientos = await _unidadTrabajo.Tratamiento.ObtenerTodosAsync(isTracking: false);

            perfilVM.Tratamientos = todosLosTratamientos.Select(t =>
            {
                var tratamientoAsignadoBD = tratamientosGuardados.FirstOrDefault(pt => pt.TratamientoRef == t.TratamientoID);
                if (tratamientoAsignadoBD != null)
                {
                    return new PerfilTratamientoFilaVM
                    {
                        TratamientoRef = t.TratamientoID,
                        TratamientoNombre = t.TratamientoNombre,
                        EstaSeleccionado = true,
                        UbicacionRef = tratamientoAsignadoBD.UbicacionRef ?? 1,
                        CantMinimaTirasStock = tratamientoAsignadoBD.CantMinimaTirasStock
                    };
                }
                else
                {
                    return new PerfilTratamientoFilaVM
                    {
                        TratamientoRef = t.TratamientoID,
                        TratamientoNombre = t.TratamientoNombre,
                        EstaSeleccionado = false,
                        UbicacionRef = 1,
                        CantMinimaTirasStock = 0
                    };
                }
            }).ToList();

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

            perfilVM.PerfilCodigoAlcemar = perfilVM.PerfilCodigoAlcemar?.Trim();
            perfilVM.Descripcion = perfilVM.Descripcion?.Trim();

            try
            {
                var existeCodigo = await _unidadTrabajo.Perfil.ObtenerTodosAsync(p =>
                    p.PerfilCodigoAlcemar == perfilVM.PerfilCodigoAlcemar &&
                    p.PerfilID != perfilVM.PerfilID);

                if (existeCodigo.Any())
                {
                    ModelState.AddModelError("PerfilCodigoAlcemar", "El código ya está siendo usado por otro perfil.");
                    await CargarListasDelViewModel(perfilVM);
                    return View(perfilVM);
                }
                var perfilOriginal = (await _unidadTrabajo.Perfil.ObtenerTodosAsync(
                    filtro: p => p.PerfilID == perfilVM.PerfilID,
                    isTracking: false
                )).FirstOrDefault();

                if (perfilOriginal == null) return NotFound();

                _mapper.Map(perfilVM, perfilOriginal);

                perfilOriginal.PesoXtira = perfilOriginal.PesoXmetro * perfilOriginal.LongTiraMts;

                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count > 0)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await archivos[0].CopyToAsync(dataStream);
                        perfilOriginal.ImagenPerfil = dataStream.ToArray(); 
                    }
                }

                _unidadTrabajo.Perfil.Actualizar(perfilOriginal);

                var tratamientosEnDb = await _unidadTrabajo.PerfilTratamiento.ObtenerTodosAsync(pt => pt.PerfilRef == perfilVM.PerfilID);
                var tratamientosTildados = perfilVM.Tratamientos != null ? perfilVM.Tratamientos.Where(t => t.EstaSeleccionado).ToList() : new List<PerfilTratamientoFilaVM>();
                var idsTratamientosTildados = tratamientosTildados.Select(t => t.TratamientoRef).ToList();
                var tratamientosDesmarcado = tratamientosEnDb.Where(pt => !idsTratamientosTildados.Contains(pt.TratamientoRef)).ToList();

                foreach (var itemAEliminar in tratamientosDesmarcado)
                {
                    _unidadTrabajo.PerfilTratamiento.Remover(itemAEliminar);
                }

                foreach (var itemPantalla in tratamientosTildados)
                {
                    var relacionExistente = tratamientosEnDb.FirstOrDefault(pt => pt.TratamientoRef == itemPantalla.TratamientoRef);

                    if (relacionExistente != null)
                    {
                        relacionExistente.UbicacionRef = itemPantalla.UbicacionRef;
                        relacionExistente.CantMinimaTirasStock = itemPantalla.CantMinimaTirasStock;
                        _unidadTrabajo.PerfilTratamiento.Actualizar(relacionExistente);
                    }
                    else
                    {
                        var nuevoPerfilTratamiento = new PerfilTratamiento
                        {
                            PerfilRef = perfilVM.PerfilID, 
                            TratamientoRef = itemPantalla.TratamientoRef,
                            UbicacionRef = itemPantalla.UbicacionRef,
                            CantMinimaTirasStock = itemPantalla.CantMinimaTirasStock,
                            CantidadStock = 0
                        };
                        _unidadTrabajo.PerfilTratamiento.Agregar(nuevoPerfilTratamiento);
                    }
                }

                await _unidadTrabajo.GuardarAsync();

                TempData["success"] = "Perfil y sus tratamientos actualizados correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar perfil ID {Id}", perfilVM.PerfilID);
                ModelState.AddModelError(string.Empty, "No se pudieron guardar los cambios. Verifique su conexión y los datos.");
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

            return Json(lineas.Select(l => new
            {
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

            vm.UbicacionesList = (await _unidadTrabajo.Ubicacion.ObtenerTodosAsync(isTracking: false))
                .Select(u => new SelectListItem
                {
                    Text = u.UbicacionDesc?.Trim() ?? "Sin Descripción",
                    Value = u.UbicacionID.ToString()
                })
                .OrderBy(u => u.Value == "1" ? 0 : 1)
                .ThenBy(u => u.Text)
                .ToList();

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
            if (vm.Tratamientos == null || !vm.Tratamientos.Any())
            {
                var todosLosTratamientos = await _unidadTrabajo.Tratamiento.ObtenerTodosAsync(isTracking: false);
                vm.Tratamientos = todosLosTratamientos.Select(t => new PerfilTratamientoFilaVM
                {
                    TratamientoRef = t.TratamientoID,
                    TratamientoNombre = t.TratamientoNombre,
                    EstaSeleccionado = false,
                    UbicacionRef = 1,
                    CantMinimaTirasStock = 0
                }).ToList();
            }
        }
    }
}