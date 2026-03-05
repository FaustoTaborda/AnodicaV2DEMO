using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface IPerfilRepositorio : IRepositorio<Perfil, int>
    {

        void Actualizar(Perfil perfil);
    }
}
