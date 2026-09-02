using System.Data;
using Dapper;
using SchoolEnrollmentAPI.Infrastructure;

namespace SchoolEnrollmentAPI.Repositories
{
    /// <summary>
    /// Define a operação de persistência necessária pelo caso de uso de matrícula.
    /// </summary>
    public interface IEnrollmentRepository
    {
        EnrollmentResult Create(int studentId, int classroomId);
    }

    /// <summary>
    /// Executa a transação SQL responsável por criar matrículas de forma consistente.
    /// </summary>
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly SqlConnectionFactory _connectionFactory = new SqlConnectionFactory();

        public EnrollmentResult Create(int studentId, int classroomId)
        {
            using (var connection = _connectionFactory.Create())
            {
                connection.Open();

                // O nível Serializable impede que duas requisições concorrentes validem a mesma vaga.
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    var studentActive = connection.QueryFirstOrDefault<bool?>(@"SELECT Ativo
                          FROM dbo.Aluno WITH (UPDLOCK, HOLDLOCK)
                          WHERE Id = @StudentId;",
                        new { StudentId = studentId }, transaction);

                    if (!studentActive.HasValue)
                        return EnrollmentResult.StudentNotFound;

                    if (!studentActive.Value)
                        return EnrollmentResult.StudentInactive;

                    var remainingSeats = connection.QueryFirstOrDefault<int?>(@"SELECT VagasDisponiveis
                          FROM dbo.Turma WITH (UPDLOCK, HOLDLOCK)
                          WHERE Id = @ClassroomId;",
                        new { ClassroomId = classroomId }, transaction);

                    if (!remainingSeats.HasValue)
                        return EnrollmentResult.ClassroomNotFound;

                    if (remainingSeats.Value <= 0)
                        return EnrollmentResult.NoRemainingSeats;

                    var existingEnrollmentId = connection.QueryFirstOrDefault<int?>(@"SELECT TOP (1) Id
                          FROM dbo.Matricula WITH (UPDLOCK, HOLDLOCK)
                          WHERE AlunoId = @StudentId AND TurmaId = @ClassroomId;",
                        new { StudentId = studentId, ClassroomId = classroomId }, transaction);

                    if (existingEnrollmentId.HasValue)
                        return EnrollmentResult.DuplicateEnrollment;

                    connection.Execute(@"INSERT INTO dbo.Matricula (AlunoId, TurmaId)
                          VALUES (@StudentId, @ClassroomId);",
                        new { StudentId = studentId, ClassroomId = classroomId }, transaction);

                    connection.Execute(@"UPDATE dbo.Turma
                          SET VagasDisponiveis = VagasDisponiveis - 1
                          WHERE Id = @ClassroomId;",
                        new { ClassroomId = classroomId }, transaction);

                    // Insert e update só são persistidos quando as duas etapas terminam sem erro.
                    transaction.Commit();
                    return EnrollmentResult.Created;
                }
            }
        }
    }

    /// <summary>
    /// Enumera os resultados de negócio possíveis ao tentar criar uma matrícula.
    /// </summary>
    public enum EnrollmentResult
    {
        Created,
        StudentNotFound,
        StudentInactive,
        ClassroomNotFound,
        NoRemainingSeats,
        DuplicateEnrollment
    }
}
