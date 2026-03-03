using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface IInsumoMovimientoRepositorio : IRepositorio<InsumoMovimiento, int>
    {
        void Actualizar(InsumoMovimiento movimiento);
    }
}