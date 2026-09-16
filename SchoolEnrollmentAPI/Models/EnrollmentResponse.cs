using System;

namespace SchoolEnrollmentAPI.Models
{
    /// <summary>
    /// Representa a confirmação retornada após uma matrícula concluída.
    /// </summary>
    public class EnrollmentResponse
    {
        public int StudentId { get; set; }
        public int ClassroomId { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
