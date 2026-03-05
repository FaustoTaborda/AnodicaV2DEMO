using Anodica.Modelos;
using Anodica.AccesoDatos.Repositorio.IRepositorio;

namespace Anodica.AccesoDatos.Repositorio
{
    public class PerfilTratamientoRepositorio : Repositorio<PerfilTratamiento, int>, IPerfilTratamientoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public PerfilTratamientoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(PerfilTratamiento perfilTratamiento)
        {
            var objDesdeDb = _db.PerfilTratamientos.FirstOrDefault(pt => pt.PerfilTratamientoId == perfilTratamiento.PerfilTratamientoId);
            if (objDesdeDb != null)
            {
                objDesdeDb.PerfilRef = perfilTratamiento.PerfilRef;
                objDesdeDb.TratamientoRef = perfilTratamiento.TratamientoRef;
                objDesdeDb.CantMinimaTirasStock = perfilTratamiento.CantMinimaTirasStock;
                objDesdeDb.CantidadStock = perfilTratamiento.CantidadStock;
                objDesdeDb.UbicacionRef = perfilTratamiento.UbicacionRef;
            }
        }
    }
}