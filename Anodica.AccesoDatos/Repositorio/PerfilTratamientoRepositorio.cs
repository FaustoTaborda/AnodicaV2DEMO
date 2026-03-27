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
            _db.PerfilTratamientos.Update(perfilTratamiento);
        }        
    }
}