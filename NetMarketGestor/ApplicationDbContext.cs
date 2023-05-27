using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetMarketGestor.Models;

namespace NetMarketGestor
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                //.HasKey(al => new { al.Id })
                .HasOne(u => u.Carrito)
                .WithOne(c => c.user)
                .HasForeignKey<Carrito>(c => c.UserId);

        }
        public DbSet<Carrito> Carritos { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<CarritoProducto> CarritoProductos { get; set; }
    }
}
