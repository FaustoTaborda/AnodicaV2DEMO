using Anodica.Modelos;
using Anodica.AccesoDatos.Repositorio.IRepositorio;

namespace Anodica.AccesoDatos.Repositorio
{
    public class ProveedorTipoProveedorRepositorio : Repositorio<ProveedorTipoProveedor, int>, IProveedorTipoProveedorRepositorio
    {
        private readonly ApplicationDbContext _db;

        public ProveedorTipoProveedorRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}