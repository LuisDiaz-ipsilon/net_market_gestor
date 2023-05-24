using NetMarketGestor.Models;
using NetMarketGestor.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace NetMarketGestor.DTOs
{
    public class UserCreacionDTO
    {

        [Required]
        public string Nombre { get; set; }

        public string Email { get; set; }

        [Required]
        public Carrito Carrito { get; set; }

        [Direccion]
        public String direccion { get; set; }
    }
}
