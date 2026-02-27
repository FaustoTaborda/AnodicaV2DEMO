using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Anodica.AccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task<T> ObtenerAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> ObtenerTodosAsync(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet;
            // CUESTION-1: Investigamos sobre el uso de query.AsNoTracking() acá para que listados como el Index no consuman memoria extra en el servidor. 
            // Comentado para explicacion mas del uso y de las funciones, en que momentos usarlo y en cuales no.

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (!string.IsNullOrEmpty(incluirPropiedades))
            {
                foreach (var inclProp in incluirPropiedades.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(inclProp);
                }
            }

            return await query.ToListAsync();
        }
        public void Agregar(T entidad)
        {
            dbSet.Add(entidad);
        }
        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }
    }
}