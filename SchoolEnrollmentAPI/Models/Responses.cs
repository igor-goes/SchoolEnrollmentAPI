using System.Collections.Generic;

namespace SchoolEnrollmentAPI.Models
{
    /// <summary>
    /// Encapsula uma página de resultados e os metadados de paginação.
    /// </summary>
    public class PagedResult<T>
    {
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public IEnumerable<T> Itens { get; set; }
    }
}
