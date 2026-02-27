using Microsoft.EntityFrameworkCore;
using Anodica.Modelos;

namespace Anodica.AccesoDatos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Insumo> Insumos { get; set; } 
    }
}