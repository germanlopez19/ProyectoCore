using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Paginacion
{
    public interface IPaginacion
    {
        Task<PaginacionModel> devolverPaginacion(
            string storePrecedure,
            int numeroPagina,
            int cantidadElementos,
            IDictionary<string, object> parametrosFiltro,
            string ordenamientoColumna
            ); 
    }
}
