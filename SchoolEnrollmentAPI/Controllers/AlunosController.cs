using System.Web.Http;
using SchoolEnrollmentAPI.Models;
using SchoolEnrollmentAPI.Services;

namespace SchoolEnrollmentAPI.Controllers
{
    [RoutePrefix("api/alunos")]
    public class AlunosController : ApiController
    {
        private readonly AlunoService _studentService = new AlunoService();

        [HttpGet, Route("")]
        public IHttpActionResult GetAll(string name = null, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("page deve ser maior que zero e pageSize deve estar entre 1 e 100.");

            return Ok(_studentService.Listar(name, page, pageSize));
        }

        [HttpGet, Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var student = _studentService.Obter(id);
            return student == null ? (IHttpActionResult)NotFound() : Ok(student);
        }

        [HttpPost, Route("")]
        public IHttpActionResult Create(AlunoRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var student = _studentService.Criar(request);
            return CreatedAtRoute("DefaultApi", new { controller = "alunos", id = student.Id }, student);
        }

        [HttpPut, Route("{id:int}")]
        public IHttpActionResult Update(int id, AlunoRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return _studentService.Atualizar(id, request) ? (IHttpActionResult)Ok(_studentService.Obter(id)) : NotFound();
        }

        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            // A exclusão é lógica para preservar o histórico escolar.
            return _studentService.Excluir(id) ? (IHttpActionResult)Ok() : NotFound();
        }
    }
}
