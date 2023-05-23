using System.ComponentModel.DataAnnotations;

namespace NetMarketGestor.Models
{
    public class Carrito
    {

        public int id { get; set; }

        [Required]
        public User user { get; set; }
        public List<Product> productos { get; set; }



    }
}
