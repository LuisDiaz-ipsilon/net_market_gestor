using System.ComponentModel.DataAnnotations;


namespace NetMarketGestor.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Categoria es requerido.")]
        public string Categoria { get; set; }

        public double Precio { get; set; }

        public string RutaImagen { get; set; }
    }
}
