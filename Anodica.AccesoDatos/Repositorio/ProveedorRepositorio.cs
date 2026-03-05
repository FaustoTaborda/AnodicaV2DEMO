using Anodica.Modelos;
using Anodica.AccesoDatos.Repositorio.IRepositorio;

namespace Anodica.AccesoDatos.Repositorio
{
    public class ProveedorRepositorio : Repositorio<Proveedor, int>, IProveedorRepositorio
    {
        private readonly ApplicationDbContext _db;

        public ProveedorRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Proveedor proveedor)
        {
            var objDesdeDb = _db.Proveedores.FirstOrDefault(p => p.ProveedorID == proveedor.ProveedorID);
            if (objDesdeDb != null)
            {
                objDesdeDb.ProveedorNombre = proveedor.ProveedorNombre;
                objDesdeDb.Telefonos = proveedor.Telefonos;
                objDesdeDb.Email = proveedor.Email;
                objDesdeDb.Productos = proveedor.Productos;
                objDesdeDb.PorcentajePesoTiraPerfil = proveedor.PorcentajePesoTiraPerfil;
            }
        }
    }
}