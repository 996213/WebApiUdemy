using System.ComponentModel.DataAnnotations;
using WebApiMoviesUdemy.Enums;

namespace WebApiMoviesUdemy.Validaciones
{
    public class TipoArchivoValidacion : ValidationAttribute
    {
        private readonly string[] tiposValidos;

        public TipoArchivoValidacion(string[] tiposValidos)
        {
            this.tiposValidos = tiposValidos;
        }

        public TipoArchivoValidacion(GrupoTipoArchivo grupoTipoArchivo)
        {
            if(grupoTipoArchivo == GrupoTipoArchivo.imagen)
            {
                this.tiposValidos = new string[] { "image/png", "image/jpeg", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;
            if (formFile == null)
                return ValidationResult.Success;


            if (!tiposValidos.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo de archivo debe debe ser uno de los siguientes  {string.Join(",", tiposValidos )}");
            }


            return ValidationResult.Success;
        }
    }
}
