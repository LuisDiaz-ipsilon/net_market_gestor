using System.ComponentModel.DataAnnotations;

namespace NetMarketGestor.DTOs
{
    public class CredencialesUsuario
    {
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} solo puede tener hasta 250 caracteres")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}