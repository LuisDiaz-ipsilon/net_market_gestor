using System.ComponentModel.DataAnnotations;
//using NetMarketGestor.Validaciones;

namespace NetMarketGestor.Models
{
    public class User
    {

        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public Carrito Carrito { get; set; }

        public String direccion { get; set; }




    }
}
