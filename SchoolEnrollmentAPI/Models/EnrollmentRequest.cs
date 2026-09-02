using System.ComponentModel.DataAnnotations;

namespace SchoolEnrollmentAPI.Models
{
    /// <summary>
    /// Define os identificadores necessários para solicitar uma nova matrícula.
    /// </summary>
    public class EnrollmentRequest
    {
        [Range(1, int.MaxValue)]
        public int StudentId { get; set; }

        [Range(1, int.MaxValue)]
        public int ClassroomId { get; set; }
    }
}
