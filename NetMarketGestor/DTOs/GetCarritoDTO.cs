using System.Collections.Generic;
using NetMarketGestor.DTOs;

namespace NetMarketGestor.DTOs
{
    public class GetCarritoDTO
    {
        public int Id { get; set; }

        public GetUserDTO User { get; set; }

        public List<ProductDTO> Productos { get; set; }
    }
}
