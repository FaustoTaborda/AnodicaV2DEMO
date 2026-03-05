using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface IPerfilTratamientoRepositorio : IRepositorio<PerfilTratamiento, int>
    {

        void Actualizar(PerfilTratamiento perfilTratamiento);
    }
}
