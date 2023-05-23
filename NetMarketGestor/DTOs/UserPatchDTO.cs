using System.ComponentModel.DataAnnotations;
using NetMarketGestor.Models;
using NetMarketGestor.Validaciones;

namespace NetMarketGestor.DTOs
{
    public class UserPatchDTO
    {

        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Email { get; set; }

        [Direccion]
        public String direccion { get; set; }
    }
}
