using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public IInsumoRepositorio Insumo { get; private set; }
        public IInsumoMovimientoRepositorio InsumoMovimiento { get; private set; }
        public IPerfilRepositorio Perfil { get; private set; }
        public ILineaRepositorio Linea { get; private set; }
        public IUbicacionRepositorio Ubicacion { get; private set; }
        public ITratamientoRepositorio Tratamiento { get; private set; }
        public ITipoProveedorRepositorio TipoProveedor { get; private set; }
        public IProveedorRepositorio Proveedor { get; private set; }
        public IPerfilTratamientoRepositorio PerfilTratamiento { get; private set; }
        public IProveedorTipoProveedorRepositorio ProveedorTipoProveedor { get; private set; }
        public IPerfilEquivalenciaRepositorio PerfilEquivalencia { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Insumo = new InsumoRepositorio(_db);
            InsumoMovimiento = new InsumoMovimientoRepositorio(_db);
            Perfil = new PerfilRepositorio(_db);
            Linea = new LineaRepositorio(_db);
            Ubicacion = new UbicacionRepositorio(_db);
            Tratamiento = new TratamientoRepositorio(_db);
            TipoProveedor = new TipoProveedorRepositorio(_db);
            Proveedor = new ProveedorRepositorio(_db);
            PerfilTratamiento = new PerfilTratamientoRepositorio(_db);
            ProveedorTipoProveedor = new ProveedorTipoProveedorRepositorio(_db);
            PerfilEquivalencia = new PerfilEquivalenciaRepositorio(_db);
        }

        public async Task GuardarAsync()
        {
            await _db.SaveChangesAsync(); 
        }

        public void Dispose() { _db.Dispose(); }
    }
}