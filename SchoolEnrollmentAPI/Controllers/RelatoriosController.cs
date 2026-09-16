using System.Web.Http;
using SchoolEnrollmentAPI.Services;

namespace SchoolEnrollmentAPI.Controllers
{
    /// <summary>
    /// Expõe os relatórios de consulta da escola pela rota /api/relatorios.
    /// </summary>
    [RoutePrefix("api/relatorios")]
    public class RelatoriosController : ApiController
    {
        private readonly ReportService _reportService = new ReportService();

        [HttpGet]
        [Route("alunos-por-turma")]
        public IHttpActionResult GetStudentsByClassroom()
        {
            return Ok(_reportService.GetStudentsByClassroom());
        }
    }
}
