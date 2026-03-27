using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;
using AnodicaV2DEMO.ViewModels;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var queryBorrador = _unidadTrabajo.Perfil.ConsultarQuery();
            var perfiles = await queryBorrador.ProjectToType<PerfilIndexVM>().ToListAsync(); 

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
                var vistaErrorCE = await ValidaExisteCodigo(perfilVM);
                if (vistaErrorCE != null) return vistaErrorCE;

                Perfil perfilParaBD = new Perfil();

                await SincronizarPerfilDesdeVM(perfilVM, perfilParaBD, HttpContext.Request.Form.Files);

                _unidadTrabajo.Perfil.Agregar(perfilParaBD);
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
                incluirPropiedades: "Linea,PerfilTratamientos"
            )).FirstOrDefault();

            if (perfilOriginal == null) return NotFound();

            PerfilVM perfilVM = _mapper.Map<PerfilVM>(perfilOriginal);
            perfilVM.ProveedorId = perfilOriginal.Linea?.ProveedorRef;

            var tratamientosGuardados = perfilOriginal.PerfilTratamientos;
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
               var vistaErrorCE = await ValidaExisteCodigo(perfilVM, perfilVM.PerfilID);
               if(vistaErrorCE !=null) return vistaErrorCE;

                var perfilOriginal = (await _unidadTrabajo.Perfil.ObtenerTodosAsync(
                    filtro: p => p.PerfilID == perfilVM.PerfilID,
                    incluirPropiedades: "PerfilTratamientos",
                    isTracking: true
                )).FirstOrDefault();

                if (perfilOriginal == null) return NotFound();

                await SincronizarPerfilDesdeVM (perfilVM, perfilOriginal, HttpContext.Request.Form.Files);

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

        private async Task SincronizarPerfilDesdeVM(PerfilVM perfilVM, Perfil perfilBD, IFormFileCollection archivos)
        {
            _mapper.Map(perfilVM, perfilBD);

            if (archivos.Count>0)
            {
                using (var dataStream = new MemoryStream())
                {
                    await archivos[0].CopyToAsync(dataStream);
                    perfilBD.ImagenPerfil = dataStream.ToArray();
                }
            }
        }

        private async Task<IActionResult> ValidaExisteCodigo(PerfilVM perfilVM, int idExcluido = 0)
        {
            var existe = _unidadTrabajo.Perfil.ConsultarQuery(p =>
        p.PerfilCodigoAlcemar == perfilVM.PerfilCodigoAlcemar &&
        p.PerfilID != idExcluido);

            if (await existe.AnyAsync())
            {
                ModelState.AddModelError("PerfilCodigoAlcemar", "El código ya está siendo usado por otro perfil.");
                await CargarListasDelViewModel(perfilVM);
                return View(perfilVM);
            }
            return null;
        }
    }
}