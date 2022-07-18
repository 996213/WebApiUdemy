using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace WebApiMoviesUdemy.Servicios
{
    public class AlmacenadorArchivosAzure : IAlmacenadorArchivos
    {
        private readonly string connectionString;

        public AlmacenadorArchivosAzure(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task BorrarArchivo(string contenedor, string ruta)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return;
            }
            var cliente = new BlobContainerClient(connectionString, contenedor);
            //Crear contenedor si no existe
            await cliente.CreateIfNotExistsAsync();

            var archivo = Path.GetFileName(ruta);
            var blob  = cliente.GetBlobClient(archivo);
            await blob.DeleteIfExistsAsync();

        }

        public async Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType)
        {
            await BorrarArchivo( contenedor, ruta);
            return await GuardarArchivo(contenido, extension, contenedor, contentType);
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType)
        {
            var cliente = new BlobContainerClient(connectionString, contenedor);
            //Crear contenedor si no existe
            await cliente.CreateIfNotExistsAsync();
            await cliente.SetAccessPolicyAsync(PublicAccessType.Blob);
            var archivoNombre = $"{ Guid.NewGuid() }{ extension }";
            var blob = cliente.GetBlobClient(archivoNombre);

            //Identificar con que archivo se va a trabajar
            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeader = new BlobHttpHeaders();
            blobHttpHeader.ContentType = contentType;   
            blobUploadOptions.HttpHeaders = blobHttpHeader;
            await blob.UploadAsync( new BinaryData(contenido), blobUploadOptions);
            return blob.Uri.ToString();

        }
    }
}
