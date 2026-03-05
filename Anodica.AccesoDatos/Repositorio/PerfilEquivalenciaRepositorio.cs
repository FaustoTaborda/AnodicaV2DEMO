using Anodica.Modelos;
using Anodica.AccesoDatos.Repositorio.IRepositorio;

namespace Anodica.AccesoDatos.Repositorio
{
    public class PerfilEquivalenciaRepositorio : Repositorio<PerfilEquivalencia, int>, IPerfilEquivalenciaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public PerfilEquivalenciaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}