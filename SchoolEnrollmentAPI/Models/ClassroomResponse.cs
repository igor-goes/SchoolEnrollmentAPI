namespace SchoolEnrollmentAPI.Models
{
    /// <summary>
    /// Define o contrato retornado pela listagem de turmas.
    /// </summary>
    public class ClassroomResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public int TotalSeats { get; set; }
        public int RemainingSeats { get; set; }
    }
}
