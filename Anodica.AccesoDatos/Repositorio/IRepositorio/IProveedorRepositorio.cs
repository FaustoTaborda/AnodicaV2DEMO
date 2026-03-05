using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProveedorRepositorio : IRepositorio<Proveedor, int>
    {

        void Actualizar(Proveedor proveedor);
    }
}
