using System.Collections.Generic;
using SchoolEnrollmentAPI.Models;
using SchoolEnrollmentAPI.Repositories;

namespace SchoolEnrollmentAPI.Services
{
    /// <summary>
    /// Expõe os casos de uso relacionados à consulta de turmas.
    /// </summary>
    public class ClassroomService
    {
        private readonly ClassroomRepository _classroomRepository = new ClassroomRepository();

        public IEnumerable<ClassroomResponse> GetAll()
        {
            return _classroomRepository.GetAll();
        }
    }
}
