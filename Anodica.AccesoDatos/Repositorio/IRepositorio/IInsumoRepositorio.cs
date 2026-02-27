using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface IInsumoRepositorio : IRepositorio<Insumo>
    {
        void Actualizar(Insumo insumo);
    }
}