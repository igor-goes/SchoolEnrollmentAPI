using System.Web.Http;

namespace SchoolEnrollmentAPI.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            // A raiz existe apenas para orientar quem abrir a URL da API no navegador.
                return Ok(new
            {
                name = "School Enrollment API",
                students = "/api/alunos",
                documentation = "/swagger"
            });
        }
    }
}
