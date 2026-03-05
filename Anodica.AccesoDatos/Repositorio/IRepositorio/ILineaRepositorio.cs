using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface ILineaRepositorio : IRepositorio<Linea, short>
    {

        void Actualizar(Linea linea);
    }
}
