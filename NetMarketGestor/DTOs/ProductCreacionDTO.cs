using System.ComponentModel.DataAnnotations;

namespace NetMarketGestor.DTOs
{
    public class ProductCreacionDTO
    {

        public string Nombre { get; set; }

        public int Existencia { get; set; }

        [Required]
        //[Categoria]
        public string Categoria { get; set; }

        public double Precio { get; set; }

        public string RutaImagen { get; set; }
    }
}
