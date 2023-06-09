﻿using System.ComponentModel.DataAnnotations;

namespace NetMarketGestor.DTOs
{
    public class GetProductDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Categoria es requerido.")]
        public string Categoria { get; set; }

        public double Precio { get; set; }

        public int Existencia { get; set; }

        public string RutaImagen { get; set; }
    }
}
