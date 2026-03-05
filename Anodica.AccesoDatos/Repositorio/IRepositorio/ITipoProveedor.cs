using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface ITipoProveedorRepositorio : IRepositorio<TipoProveedor, byte>
    {

        void Actualizar(TipoProveedor tipoProveedor);
    }
}
