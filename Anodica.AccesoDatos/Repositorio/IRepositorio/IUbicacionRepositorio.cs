using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUbicacionRepositorio : IRepositorio<Ubicacion, short>
    {

        void Actualizar(Ubicacion ubicacion);
    }
}
