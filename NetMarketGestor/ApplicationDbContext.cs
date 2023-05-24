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
                .HasKey(al => new { al.Email, al.Nombre });
        }
        public DbSet<Carrito> Carritos { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
