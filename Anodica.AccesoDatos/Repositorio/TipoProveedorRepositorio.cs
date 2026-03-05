using Anodica.Modelos;
using Anodica.AccesoDatos.Repositorio.IRepositorio;

namespace Anodica.AccesoDatos.Repositorio
{
    public class TipoProveedorRepositorio : Repositorio<TipoProveedor, byte>, ITipoProveedorRepositorio
    {
        private readonly ApplicationDbContext _db;

        public TipoProveedorRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(TipoProveedor tipoProveedor)
        {
            var objDesdeDb = _db.TiposProveedor.FirstOrDefault(t => t.TipoProveedorID == tipoProveedor.TipoProveedorID);
            if (objDesdeDb != null)
            {
                objDesdeDb.TipoProveedorNombre = tipoProveedor.TipoProveedorNombre;
            }
        }
    }
}