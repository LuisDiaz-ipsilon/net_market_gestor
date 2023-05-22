﻿using System.ComponentModel.DataAnnotations;
using NetMarketGestor.Validaciones;

namespace NetMarketGestor.Models
{
    public class User
    {

        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Email { get; set; }

        [Required]
        public Carrito Carrito { get; set; }

        [Direccion]
        public String direccion { get; set; }




    }
}
