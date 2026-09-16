using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolEnrollmentAPI.Models
{
    /// <summary>
    /// Define os dados aceitos para criação e atualização de alunos.
    /// </summary>
    public class AlunoRequest
    {
        [Required, StringLength(150)]
        public string Nome { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [Required]
        public DateTime? DataNascimento { get; set; }
    }
}
