using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio
{
    public class InsumoRepositorio : Repositorio<Insumo>, IInsumoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public InsumoRepositorio(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Actualizar(Insumo insumo)
        {
            var objDesdeDb = _db.Insumos.FirstOrDefault(s => s.InsumoID == insumo.InsumoID);
            if (objDesdeDb != null)
            {
                objDesdeDb.InsumoNombre = insumo.InsumoNombre;
                objDesdeDb.CodigoInsumo = insumo.CodigoInsumo;
                objDesdeDb.UnidadMedida = insumo.UnidadMedida;
                objDesdeDb.CantidadStock = insumo.CantidadStock;   
                objDesdeDb.CantMinimaStock = insumo.CantMinimaStock;
                
            }
        }
    }
}