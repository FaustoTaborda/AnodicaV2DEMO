using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio
{
    public class PerfilRepositorio : Repositorio<Perfil, int>, IPerfilRepositorio
    {
        private readonly ApplicationDbContext _db;

        public PerfilRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Perfil perfil)
        {
            _db.Perfiles.Update(perfil);
        }
    }
}