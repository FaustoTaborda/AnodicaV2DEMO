using Anodica.Modelos;
using Anodica.AccesoDatos.Repositorio.IRepositorio;

namespace Anodica.AccesoDatos.Repositorio
{
    public class UbicacionRepositorio : Repositorio<Ubicacion, short>, IUbicacionRepositorio
    {
        private readonly ApplicationDbContext _db;

        public UbicacionRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Ubicacion ubicacion)
        {
            var objDesdeDb = _db.Ubicaciones.FirstOrDefault(u => u.UbicacionID == ubicacion.UbicacionID);
            if (objDesdeDb != null)
            {
                objDesdeDb.UbicacionCodigo = ubicacion.UbicacionCodigo;
                objDesdeDb.UbicacionDesc = ubicacion.UbicacionDesc;
            }
        }
    }
}