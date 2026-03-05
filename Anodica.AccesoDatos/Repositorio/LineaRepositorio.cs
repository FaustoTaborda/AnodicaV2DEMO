using Anodica.Modelos;
using Anodica.AccesoDatos.Repositorio.IRepositorio;

namespace Anodica.AccesoDatos.Repositorio
{
    public class LineaRepositorio : Repositorio<Linea, short>, ILineaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public LineaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Linea linea)
        {
            var objDesdeDb = _db.Lineas.FirstOrDefault(l => l.LineaID == linea.LineaID);
            if (objDesdeDb != null)
            {
                objDesdeDb.LineaNombre = linea.LineaNombre;
                objDesdeDb.ProveedorRef = linea.ProveedorRef;
                objDesdeDb.LineaGrupoRef = linea.LineaGrupoRef;
            }
        }
    }
}