using System.ComponentModel.DataAnnotations;

namespace NetMarketGestor.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}