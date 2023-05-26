using System.ComponentModel.DataAnnotations;
using NetMarketGestor.Validaciones;

namespace NetMarketGestor.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo DireccionEntrega es requerido.")]
        [StringLength(150, ErrorMessage = "El campo DireccionEntrega puede tener hasta 150 caracteres.")]
        [Direccion]
        public string DireccionEntrega { get; set; }

        [Required(ErrorMessage = "El campo MetodoPago es requerido.")]
        [MetodoPago]
        public string MetodoPago { get; set; }

        public string Estatus { get; set; }

        public int carritoId { get; set; }

        public int userId { get; set; }

    }
}
