using System.ComponentModel.DataAnnotations;

namespace NetMarketGestor.DTOs
{
    public class GetUserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Direccion { get; set; }
    }
}
