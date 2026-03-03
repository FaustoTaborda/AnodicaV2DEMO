namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        IInsumoRepositorio Insumo { get; }

        IInsumoMovimientoRepositorio InsumoMovimiento { get; }
        Task GuardarAsync(); 
    }
}
