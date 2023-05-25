using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using NetMarketGestor.Validaciones;

namespace NetMarketGestor.Models
{
    public class User
    {

        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Email { get; set; }

       
        public Carrito Carrito { get; set; }

        public List<Pedido> Pedidos { get; set; }

        [Direccion]
        public string direccion { get; set; }




    }
}
