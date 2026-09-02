using System.Collections.Generic;
using SchoolEnrollmentAPI.Models;
using SchoolEnrollmentAPI.Repositories;

namespace SchoolEnrollmentAPI.Services
{
    /// <summary>
    /// Expõe os casos de uso de relatórios sem acoplar os controllers ao SQL.
    /// </summary>
    public class ReportService
    {
        private readonly ReportRepository _reportRepository = new ReportRepository();

        public IEnumerable<ClassroomReportResponse> GetStudentsByClassroom()
        {
            return _reportRepository.GetStudentsByClassroom();
        }
    }
}
