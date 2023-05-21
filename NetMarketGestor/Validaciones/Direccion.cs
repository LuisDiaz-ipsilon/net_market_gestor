using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NetMarketGestor.Validaciones
{
    public class Direccion : ValidationAttribute
    {
        private readonly string _regexPattern;

        public Direccion()
        {
            _regexPattern = @"^[A-Za-z]+\s+\d+\s+[A-Za-z]+\s+[A-Za-z]+\s+\d{5}$";
            ErrorMessage = "La dirección de domicilio no es válida";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string direccion = value.ToString();

            if (!Regex.IsMatch(direccion, _regexPattern))
            {
                return new ValidationResult("Debe de tener el formato: calle 1234 municipio estado 12345");
            }

            return ValidationResult.Success;
        }
    }
}
