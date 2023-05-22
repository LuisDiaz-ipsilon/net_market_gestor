using System.ComponentModel.DataAnnotations;
using NetMarketGestor.Models;
using NetMarketGestor.Validaciones;

namespace NetMarketGestor.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string Nombre { get; set; }

        public string Email { get; set; }   

        [Required(ErrorMessage = "El campo Carrito es requerido.")]
        public CarritoDTO Carrito { get; set; }

        public List<Pedido> Pedidos { get; set; }

        [Direccion]
        public string Direccion { get; set; }
    }
}
