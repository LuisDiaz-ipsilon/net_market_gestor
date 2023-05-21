using Microsoft.EntityFrameworkCore;
using NetMarketGestor.Models;

namespace NetMarketGestor
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Carrito> Carritos { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
