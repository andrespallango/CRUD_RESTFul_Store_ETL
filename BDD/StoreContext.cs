using Microsoft.EntityFrameworkCore;

namespace BDD
{
    public class StoreContext : DbContext{
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {}
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Celular> Celulares { get; set; }
        public DbSet<Computadora> Computadoras { get; set; }
        public DbSet<LineaBlanca> LineasBlancas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Categoria>().ToTable("Categorias");
            modelBuilder.Entity<Producto>().ToTable("Productos");
            modelBuilder.Entity<Venta>().ToTable("Ventas");
            modelBuilder.Entity<Celular>().ToTable("Celulares");
            modelBuilder.Entity<Computadora>().ToTable("Computadoras");
            modelBuilder.Entity<LineaBlanca>().ToTable("LineasBlancas");
        }
    }
}
