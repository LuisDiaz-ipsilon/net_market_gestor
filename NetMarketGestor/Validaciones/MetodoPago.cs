using System.ComponentModel.DataAnnotations;

namespace NetMarketGestor.Validaciones
{
    public class MetodoPago : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("El campo de método de pago es requerido");
            }

            string metodoPago = value.ToString().ToLower();

            if (metodoPago != "tarjeta" && metodoPago != "bitcoin" && metodoPago != "efectivo")
            {
                return new ValidationResult("El método de pago debe ser 'tarjeta', 'bitcoin' o 'efectivo'");
            }

            return ValidationResult.Success;
        }
    }
}
