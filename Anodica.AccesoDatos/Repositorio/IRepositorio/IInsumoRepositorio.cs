using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface IInsumoRepositorio : IRepositorio<Insumo,short>
    {
        void Actualizar(Insumo insumo);
    }
}