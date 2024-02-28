using Microsoft.EntityFrameworkCore;

namespace BDD
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        //Agregar las clases para el proyecto aquí
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cerveza> Cervezas { get; set; }
        public DbSet<Jugo> Jugos { get; set; }
        public DbSet<Vino> Vinos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().ToTable("Categoria");
            modelBuilder.Entity<Cerveza>().ToTable("Cerveza");
            modelBuilder.Entity<Vino>().ToTable("Vino");
            modelBuilder.Entity<Jugo>().ToTable("Jugo");
        }
    }
}