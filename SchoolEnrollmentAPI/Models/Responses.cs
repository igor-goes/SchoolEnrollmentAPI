using System.Collections.Generic;

namespace SchoolEnrollmentAPI.Models
{
    public class PagedResult<T>
    {
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public IEnumerable<T> Itens { get; set; }
    }
}
