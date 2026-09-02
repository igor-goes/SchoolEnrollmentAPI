using System.Web.Http;
using SchoolEnrollmentAPI.Services;

namespace SchoolEnrollmentAPI.Controllers
{
    [RoutePrefix("api/turmas")]
    public class TurmasController : ApiController
    {
        private readonly ClassroomService _classroomService = new ClassroomService();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_classroomService.GetAll());
        }
    }
}
