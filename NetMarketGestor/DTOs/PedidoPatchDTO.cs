using System.ComponentModel.DataAnnotations;
using NetMarketGestor.Models;
using NetMarketGestor.Validaciones;

namespace NetMarketGestor.DTOs
{
    public class PedidoPatchDTO
    {
        public int id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} solo puede tener hasta 150 caracteres")]
        [Direccion]
        public string DireccionEntrega { get; set; }

        public string Estatus { get; set; }


    }
}
