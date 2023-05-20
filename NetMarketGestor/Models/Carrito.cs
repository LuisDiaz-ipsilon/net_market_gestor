namespace NetMarketGestor.Models
{
    public class Carrito
    {

        public int id { get; set; }

        public User user { get; set; }
        public List<Product> productos { get; set; }



    }
}
