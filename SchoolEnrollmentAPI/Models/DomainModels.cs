using System;

namespace SchoolEnrollmentAPI.Models
{
    /// <summary>
    /// Representa os dados persistidos de um aluno na tabela Aluno.
    /// </summary>
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; }
    }
}
