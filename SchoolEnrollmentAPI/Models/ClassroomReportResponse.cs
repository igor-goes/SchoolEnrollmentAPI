namespace SchoolEnrollmentAPI.Models
{
    /// <summary>
    /// Define os indicadores de matrícula consolidados para cada turma.
    /// </summary>
    public class ClassroomReportResponse
    {
        public string ClassroomName { get; set; }
        public int EnrolledStudentsCount { get; set; }
        public int RemainingSeats { get; set; }
    }
}
