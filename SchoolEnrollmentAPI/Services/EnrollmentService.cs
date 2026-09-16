using System;
using System.Net;
using SchoolEnrollmentAPI.Models;
using SchoolEnrollmentAPI.Repositories;

namespace SchoolEnrollmentAPI.Services
{
    /// <summary>
    /// Aplica as regras de negócio de matrícula e traduz seus resultados para a camada HTTP.
    /// </summary>
    public class EnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService()
            : this(new EnrollmentRepository())
        {
        }

        // Permite testar as regras sem abrir conexão com o banco de dados.
        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public EnrollmentResponse Create(EnrollmentRequest request)
        {
            var result = _enrollmentRepository.Create(request.StudentId, request.ClassroomId);

            switch (result)
            {
                case EnrollmentResult.Created:
                    return new EnrollmentResponse
                    {
                        StudentId = request.StudentId,
                        ClassroomId = request.ClassroomId,
                        EnrolledAt = DateTime.UtcNow
                    };
                case EnrollmentResult.StudentNotFound:
                    throw new EnrollmentException("Aluno não encontrado.", HttpStatusCode.NotFound);
                case EnrollmentResult.ClassroomNotFound:
                    throw new EnrollmentException("Turma não encontrada.", HttpStatusCode.NotFound);
                case EnrollmentResult.StudentInactive:
                    throw new EnrollmentException("O aluno está inativo.", HttpStatusCode.Conflict);
                case EnrollmentResult.NoRemainingSeats:
                    throw new EnrollmentException("A turma não possui vagas disponíveis.", HttpStatusCode.Conflict);
                default:
                    throw new EnrollmentException("O aluno já está matriculado nesta turma.", HttpStatusCode.Conflict);
            }
        }
    }
}
