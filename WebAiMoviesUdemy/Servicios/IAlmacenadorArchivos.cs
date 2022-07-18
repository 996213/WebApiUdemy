namespace WebApiMoviesUdemy.Servicios
{
    public interface IAlmacenadorArchivos
    {
        //Contenedor es una carpeta en azure,donde se guardan archivos relacionados.
        Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType);
        Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType);
        Task BorrarArchivo(string contenedor, string ruta);
    }
}
