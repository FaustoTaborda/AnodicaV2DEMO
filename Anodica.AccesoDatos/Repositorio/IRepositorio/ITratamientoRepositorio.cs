using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface ITratamientoRepositorio : IRepositorio<Tratamiento, short>
    {

        void Actualizar(Tratamiento tratamiento);
    }
}
