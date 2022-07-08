using System.ComponentModel.DataAnnotations;

namespace WebApiMoviesUdemy.Validaciones
{
    public class PesoArchivoValidacion : ValidationAttribute
    {
        private readonly int pesoMaximoEnBegaBytes;

        public PesoArchivoValidacion(int pesoMaximoEnBegaBytes)
        {
            this.pesoMaximoEnBegaBytes = pesoMaximoEnBegaBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;
            if (formFile == null)
                return ValidationResult.Success;


            if (formFile.Length > pesoMaximoEnBegaBytes * 1024 * 1024)
                return new ValidationResult($"El peso del archivo no debe ser superior a  {pesoMaximoEnBegaBytes }");


            return ValidationResult.Success;
        }

    }
}
