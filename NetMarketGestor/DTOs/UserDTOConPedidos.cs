namespace NetMarketGestor.DTOs
{
    public class UserDTOConPedidos: GetUserDTO
    {

        public List<PedidoDTO> Pedidos { get; set; }
    }
}
