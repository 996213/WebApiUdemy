using Microsoft.EntityFrameworkCore;

namespace WebApiMoviesUdemy.Helpers
{
    public static class HttpContextExtension
    {
        //Acceso a la cabecera de la petición
        //queryable cantidad total de registros
        public async static Task InsertarParametrosPaginacion<T>(this HttpContext httpContext,
            IQueryable<T> queryable, int cantidadRegistrosPorPagina)
        {
            double cantidad = await queryable.CountAsync();
            double cantidadPaginas = Math.Ceiling(cantidad/cantidadRegistrosPorPagina);
            httpContext.Response.Headers.Add("cantidadPaginas", cantidadPaginas.ToString()); 
        }
    }
}
