using Microsoft.EntityFrameworkCore;
using Anodica.Modelos;

namespace Anodica.AccesoDatos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<InsumoMovimiento> InsumoMovimientos { get; set; }
        public DbSet<Linea> Lineas { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<TipoProveedor> TiposProveedor { get; set; }
        public DbSet<Perfil> Perfiles { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<ProveedorTipoProveedor> ProveedorTipoProveedores { get; set; }
        public DbSet<PerfilTratamiento> PerfilTratamientos { get; set; }
        public DbSet<PerfilEquivalencia> PerfilEquivalencias { get; set; }
    }
}