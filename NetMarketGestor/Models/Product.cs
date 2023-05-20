using System.ComponentModel.DataAnnotations;
using System.Drawing;
//using NetMarketGestor.Validaciones;

using System.ComponentModel;

namespace NetMarketGestor.Models
{
    public class Product
    {
        public int id { get; set; }

        [Required]
        //[Categoria]
        public string Categoria { get; set; }

        public double Precio { get; set; }

        public string RutaImagen { get; set; }

    }
}
