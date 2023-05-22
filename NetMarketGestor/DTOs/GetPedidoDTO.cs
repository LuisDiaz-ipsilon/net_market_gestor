using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetMarketGestor.DTOs;

namespace NetMarketGestor.DTOs
{
    public class GetPedidoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo DireccionEntrega es requerido.")]
        [StringLength(150, ErrorMessage = "El campo DireccionEntrega puede tener hasta 150 caracteres.")]
        public string DireccionEntrega { get; set; }

        [Required(ErrorMessage = "El campo MetodoPago es requerido.")]
        public string MetodoPago { get; set; }

        public string Estatus { get; set; }

        public List<ProductDTO> Productos { get; set; }

        public GetUserDTO User { get; set; }
    }
}
