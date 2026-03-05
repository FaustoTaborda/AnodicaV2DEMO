using Anodica.Modelos;
using Anodica.AccesoDatos.Repositorio.IRepositorio;

namespace Anodica.AccesoDatos.Repositorio
{
    public class TratamientoRepositorio : Repositorio<Tratamiento, short>, ITratamientoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public TratamientoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Tratamiento tratamiento)
        {
            var objDesdeDb = _db.Tratamientos.FirstOrDefault(t => t.TratamientoID == tratamiento.TratamientoID);
            if (objDesdeDb != null)
            {
                objDesdeDb.TratamientoNombre = tratamiento.TratamientoNombre;
                objDesdeDb.TratamientoColorFondo = tratamiento.TratamientoColorFondo;
                objDesdeDb.TratamientoColorFuente = tratamiento.TratamientoColorFuente;
                objDesdeDb.PrecioXKgTratamiento = tratamiento.PrecioXKgTratamiento;
                objDesdeDb.Orden = tratamiento.Orden;
            }
        }
    }
}