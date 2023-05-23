using System.ComponentModel.DataAnnotations;
using NetMarketGestor.Models;
using NetMarketGestor.Validaciones;

namespace NetMarketGestor.DTOs
{
    public class ProductPatchDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int Existencia { get; set; }

        [Required]
        //[Categoria]
        public string Categoria { get; set; }

        public double Precio { get; set; }

        public string RutaImagen { get; set; }
    }
}
