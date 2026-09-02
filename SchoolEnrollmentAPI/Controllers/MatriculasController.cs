using System.Net;
using System.Web.Http;
using SchoolEnrollmentAPI.Models;
using SchoolEnrollmentAPI.Services;

namespace SchoolEnrollmentAPI.Controllers
{
    /// <summary>
    /// Expõe a criação de matrículas pela rota /api/matriculas.
    /// </summary>
    [RoutePrefix("api/matriculas")]
    public class MatriculasController : ApiController
    {
        private readonly EnrollmentService _enrollmentService = new EnrollmentService();

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(EnrollmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var enrollment = _enrollmentService.Create(request);
                return Content(HttpStatusCode.Created, enrollment);
            }
            catch (EnrollmentException exception)
            {
                return Content(exception.StatusCode, new { message = exception.Message });
            }
        }
    }
}
