using MantenimientoTrabajadores.Models;
using Microsoft.EntityFrameworkCore;

namespace MantenimientoTrabajadores.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trabajador> Trabajadores { get; set; }
    }
}