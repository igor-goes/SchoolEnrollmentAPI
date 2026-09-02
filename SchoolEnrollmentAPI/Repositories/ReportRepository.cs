using System.Collections.Generic;
using Dapper;
using SchoolEnrollmentAPI.Infrastructure;
using SchoolEnrollmentAPI.Models;

namespace SchoolEnrollmentAPI.Repositories
{
    /// <summary>
    /// Executa consultas SQL analíticas necessárias para os relatórios da API.
    /// </summary>
    public class ReportRepository
    {
        private readonly SqlConnectionFactory _connectionFactory = new SqlConnectionFactory();

        public IEnumerable<ClassroomReportResponse> GetStudentsByClassroom()
        {
            const string sql = @"
                SELECT
                    classroom.Nome AS ClassroomName,
                    -- COUNT da chave da matrícula não contabiliza a linha nula do LEFT JOIN.
                    COUNT(enrollment.Id) AS EnrolledStudentsCount,
                    classroom.VagasDisponiveis AS RemainingSeats
                FROM dbo.Turma AS classroom
                -- LEFT JOIN mantém no relatório inclusive a turma que ainda não possui matrícula.
                LEFT JOIN dbo.Matricula AS enrollment ON enrollment.TurmaId = classroom.Id
                -- O agrupamento gera um resumo por turma diretamente no SQL Server.
                GROUP BY classroom.Id, classroom.Nome, classroom.VagasDisponiveis
                ORDER BY classroom.Nome, classroom.Id;";

            using (var connection = _connectionFactory.Create())
            {
                return connection.Query<ClassroomReportResponse>(sql).AsList();
            }
        }
    }
}
