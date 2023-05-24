using NetMarketGestor.Models;
using System.ComponentModel.DataAnnotations;

namespace NetMarketGestor.DTOs
{
    public class CarritoCreacionDTO
    {
        public int id { get; set; }

        [Required]
        public User user { get; set; }
    } 
}
