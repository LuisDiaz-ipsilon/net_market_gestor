using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using NetMarketGestor.Validaciones;

namespace NetMarketGestor.Models
{
    public class Pedido
    {

        public int id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} solo puede tener hasta 150 caracteres")]
        [Direccion]
        public string DireccionEntrega { get; set; }

        [Required]
        [MetodoPago]
        public string MetodoPago { get; set; }

        public string Estatus { get; set; }

        public List<Product> Productos { get; set; }

        public User User { get; set; }







    }
}
