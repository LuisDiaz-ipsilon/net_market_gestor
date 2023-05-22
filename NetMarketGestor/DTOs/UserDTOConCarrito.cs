namespace NetMarketGestor.DTOs
{
    public class UserDTOConCarrito: GetUserDTO
    {
        public List<UserDTOConCarrito> Carrito { get; set; }
    }
}
