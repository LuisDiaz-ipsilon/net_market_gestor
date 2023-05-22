namespace NetMarketGestor.DTOs
{
    public class CarritoDTO
    {
        public int Id { get; set; }

        public UserDTO User { get; set; }

        public List<ProductDTO> Productos { get; set; }
    }
}
