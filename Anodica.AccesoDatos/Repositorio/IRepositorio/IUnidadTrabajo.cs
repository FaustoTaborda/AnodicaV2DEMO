namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        IInsumoRepositorio Insumo { get; }
        Task GuardarAsync(); 
    }
}
