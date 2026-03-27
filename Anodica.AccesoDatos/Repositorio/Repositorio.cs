using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Anodica.AccesoDatos.Repositorio
{
    public class Repositorio<T,I> : IRepositorio<T,I> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task<T> ObtenerAsync(I id)
        {
            return await dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> ObtenerTodosAsync(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null,
        bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

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
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public IQueryable<T> ConsultarQuery(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet;
            query = query.AsNoTracking();
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
            return query;
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