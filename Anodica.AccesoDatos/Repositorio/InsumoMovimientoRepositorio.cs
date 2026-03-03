using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;
using System.Linq;

namespace Anodica.AccesoDatos.Repositorio
{
    public class InsumoMovimientoRepositorio : Repositorio<InsumoMovimiento, int>, IInsumoMovimientoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public InsumoMovimientoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(InsumoMovimiento movimiento)
        {
            var objDesdeDb = _db.InsumoMovimientos.FirstOrDefault(s => s.InsumoMovimientoID == movimiento.InsumoMovimientoID);

            if (objDesdeDb != null)
            {
                objDesdeDb.InsumoRef = movimiento.InsumoRef;
                objDesdeDb.ProveedorRef = movimiento.ProveedorRef;
                objDesdeDb.OperarioRetiroRef = movimiento.OperarioRetiroRef;
                objDesdeDb.EsIngreso = movimiento.EsIngreso;
                objDesdeDb.Cantidad = movimiento.Cantidad;
                objDesdeDb.FechaMovimiento = movimiento.FechaMovimiento;

                //FechaCreacion intencionalmente omitida para que no se modifique.
            }
        }
    }
}