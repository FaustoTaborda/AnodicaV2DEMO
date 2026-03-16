using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;

namespace Anodica.AccesoDatos.Repositorio
{
    public class InsumoRepositorio : Repositorio<Insumo,short>, IInsumoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public InsumoRepositorio(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Actualizar(Insumo insumo)
        {
            _db.Insumos.Update(insumo);
        }
    }
}