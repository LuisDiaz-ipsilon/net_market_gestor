namespace NetMarketGestor.DTOs
{
    public class ProductoDTOConPedido: GetProductDTO
    {

        public List<PedidoDTO> Pedidos { get; set; }
    }
}
