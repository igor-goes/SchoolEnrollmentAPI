using System.Collections.Generic;
using SchoolEnrollmentAPI.Models;
using SchoolEnrollmentAPI.Repositories;

namespace SchoolEnrollmentAPI.Services
{
    public class ClassroomService
    {
        private readonly ClassroomRepository _classroomRepository = new ClassroomRepository();

        public IEnumerable<ClassroomResponse> GetAll()
        {
            return _classroomRepository.GetAll();
        }
    }
}
