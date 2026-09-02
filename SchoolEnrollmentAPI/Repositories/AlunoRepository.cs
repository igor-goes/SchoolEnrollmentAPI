using Dapper;
using SchoolEnrollmentAPI.Infrastructure;
using SchoolEnrollmentAPI.Models;

namespace SchoolEnrollmentAPI.Repositories
{
    public class AlunoRepository
    {
        private readonly SqlConnectionFactory _factory = new SqlConnectionFactory();

        public PagedResult<Aluno> Listar(string name, int page, int pageSize)
        {
            // COUNT e página são executados na mesma ida ao banco.
            const string sql = @"SELECT COUNT(1) FROM Aluno WHERE Ativo = 1 AND (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%'); SELECT Id, Nome, Email, DataNascimento, Ativo FROM Aluno WHERE Ativo = 1 AND (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%') ORDER BY Nome, Id OFFSET @Offset ROWS FETCH NEXT @Tamanho ROWS ONLY;";
            using (var connection = _factory.Create())
            using (var resultSets = connection.QueryMultiple(sql, new { Nome = string.IsNullOrWhiteSpace(name) ? null : name.Trim(), Offset = (page - 1) * pageSize, Tamanho = pageSize }))
            {
                return new PagedResult<Aluno> { Total = resultSets.ReadFirst<int>(), Pagina = page, TamanhoPagina = pageSize, Itens = resultSets.Read<Aluno>().AsList() };
            }
        }

        public Aluno Obter(int id)
        {
            using (var connection = _factory.Create())
                return connection.QueryFirstOrDefault<Aluno>("SELECT Id, Nome, Email, DataNascimento, Ativo FROM Aluno WHERE Id = @Id AND Ativo = 1", new { Id = id });
        }

        public int Criar(Aluno aluno)
        {
            const string sql = "INSERT INTO Aluno (Nome, Email, DataNascimento, Ativo) VALUES (@Nome, @Email, @DataNascimento, 1); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using (var connection = _factory.Create()) return connection.QuerySingle<int>(sql, aluno);
        }

        public bool Atualizar(int id, Aluno aluno)
        {
            const string sql = "UPDATE Aluno SET Nome=@Nome, Email=@Email, DataNascimento=@DataNascimento WHERE Id=@Id AND Ativo=1";
            using (var connection = _factory.Create()) return connection.Execute(sql, new { Id = id, aluno.Nome, aluno.Email, aluno.DataNascimento }) == 1;
        }

        public bool Desativar(int id)
        {
            using (var connection = _factory.Create()) return connection.Execute("UPDATE Aluno SET Ativo=0 WHERE Id=@Id AND Ativo=1", new { Id = id }) == 1;
        }
    }
}
