using System.Linq.Expressions;

namespace Anodica.AccesoDatos.Repositorio.IRepositorio
{
    public interface IRepositorio<T,I> where T : class
    {
        Task<T> ObtenerAsync(I id);

        Task<IEnumerable<T>> ObtenerTodosAsync(Expression<Func<T, bool>> filtro = null,string incluirPropiedades = null,
        bool isTracking = true);
        // CUESTION-1: Consultar agregar un parámetro bool isTracking para optimizar las consultas de solo lectura con AsNoTracking().
        void Agregar(T entidad);
        void Remover(T entidad);
    }
}