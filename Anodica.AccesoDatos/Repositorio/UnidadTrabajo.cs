using Anodica.AccesoDatos.Repositorio.IRepositorio;

namespace Anodica.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public IInsumoRepositorio Insumo { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Insumo = new InsumoRepositorio(_db);
        }

        public async Task GuardarAsync()
        {
            await _db.SaveChangesAsync(); 
        }

        public void Dispose() { _db.Dispose(); }
    }
}