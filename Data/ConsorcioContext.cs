using Microsoft.EntityFrameworkCore;
using Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        public DbSet<Sum> Sum { get; set; }
        public DbSet<ReservaSum> ReservaSum { get; set; }

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

            modelBuilder.Entity<Gasto>()
                .Property(g => g.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Unidad>()
                .HasOne(u => u.UsuarioCreador)
                .WithMany(u => u.Unidades)          // ← vincula explícitamente
                .HasForeignKey(u => u.IdUsuarioCreador)
                .OnDelete(DeleteBehavior.Restrict);


            // Un Consorcio tiene muchos Sum
            modelBuilder.Entity<Sum>()
                .HasOne(s => s.Consorcio)
                .WithMany(c => c.Sums)
                .HasForeignKey(s => s.IdConsorcio)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservaSum>()
                .HasOne(r => r.Sum)
                .WithMany(s => s.Reservas)
                .HasForeignKey(r => r.IdSum)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservaSum>()
                .HasOne(r => r.UsuarioQueReserva)
                .WithMany(u => u.Reservas)
                .HasForeignKey(r => r.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Turno como enum
            modelBuilder.Entity<ReservaSum>()
                .Property(r => r.Turno)
                .HasConversion<int>();
        }
    }
}