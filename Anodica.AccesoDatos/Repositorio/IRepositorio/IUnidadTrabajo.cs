namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        IInsumoRepositorio Insumo { get; }

        IInsumoMovimientoRepositorio InsumoMovimiento { get; }

        IPerfilRepositorio Perfil { get; }
        ILineaRepositorio Linea { get; }
        IUbicacionRepositorio Ubicacion { get; }
        ITratamientoRepositorio Tratamiento { get; }
        ITipoProveedorRepositorio TipoProveedor { get; }
        IProveedorRepositorio Proveedor { get; }
        IPerfilTratamientoRepositorio PerfilTratamiento { get; }
        IProveedorTipoProveedorRepositorio ProveedorTipoProveedor { get; }
        IPerfilEquivalenciaRepositorio PerfilEquivalencia { get; }
        Task GuardarAsync(); 
    }
}
