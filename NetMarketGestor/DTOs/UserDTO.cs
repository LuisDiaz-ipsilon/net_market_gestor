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


        [Direccion]
        public string Direccion { get; set; }
    }
}
