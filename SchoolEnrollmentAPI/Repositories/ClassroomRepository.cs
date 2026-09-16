using System.Collections.Generic;
using Dapper;
using SchoolEnrollmentAPI.Infrastructure;
using SchoolEnrollmentAPI.Models;

namespace SchoolEnrollmentAPI.Repositories
{
    /// <summary>
    /// Executa as consultas SQL de leitura das turmas cadastradas.
    /// </summary>
    public class ClassroomRepository
    {
        private readonly SqlConnectionFactory _connectionFactory = new SqlConnectionFactory();

        public IEnumerable<ClassroomResponse> GetAll()
        {
            // Os aliases mantêm o contrato da API em inglês sem alterar o schema fornecido no teste.
            const string sql = @"
                SELECT
                    Id,
                    Nome AS Name,
                    Periodo AS Period,
                    VagasTotal AS TotalSeats,
                    VagasDisponiveis AS RemainingSeats
                FROM dbo.Turma
                ORDER BY Nome, Id;";

            using (var connection = _connectionFactory.Create())
            {
                return connection.Query<ClassroomResponse>(sql).AsList();
            }
        }
    }
}
