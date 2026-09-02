using System.Web.Http;

namespace SchoolEnrollmentAPI.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            // Garante que a execução pelo IIS Express abra a documentação interativa.
            return Redirect("/swagger");
        }
    }
}
