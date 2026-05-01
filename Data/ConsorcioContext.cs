using Microsoft.EntityFrameworkCore;
using Model;

namespace Data
{
    public class ConsorcioContext : DbContext
    {
        public ConsorcioContext(DbContextOptions<ConsorcioContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Consorcio> Consorcios { get; set; }
        public DbSet<Unidad> Unidades { get; set; }
        public DbSet<TipoGasto> TiposGasto { get; set; }
        public DbSet<Gasto> Gastos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Consorcio>()
                .HasOne(c => c.UsuarioCreador)
                .WithMany(u => u.Consorcios)        // ← vincula explícitamente
                .HasForeignKey(c => c.IdUsuarioCreador)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.UsuarioCreador)
                .WithMany(u => u.Gastos)            // ← vincula explícitamente
                .HasForeignKey(g => g.IdUsuarioCreador)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Unidad>()
                .HasOne(u => u.UsuarioCreador)
                .WithMany(u => u.Unidades)          // ← vincula explícitamente
                .HasForeignKey(u => u.IdUsuarioCreador)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Gasto>()
                .Property(g => g.Monto)
                .HasPrecision(18, 2);
        }
    }
}