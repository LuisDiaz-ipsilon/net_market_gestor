using System.ComponentModel.DataAnnotations;

namespace NetMarketGestor.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        public string Nombre { get; set; }
    }
}