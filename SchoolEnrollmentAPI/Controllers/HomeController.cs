using System.Web.Http;

namespace SchoolEnrollmentAPI.Controllers
{
    /// <summary>
    /// Direciona a URL raiz da aplicação para a documentação interativa.
    /// </summary>
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            // Garante que a execução pelo IIS Express abra a documentação interativa.
            var swaggerUri = new System.Uri(Request.RequestUri, "swagger");
            return Redirect(swaggerUri);
        }
    }
}
