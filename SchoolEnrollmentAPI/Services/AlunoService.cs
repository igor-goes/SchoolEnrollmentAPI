using SchoolEnrollmentAPI.Models;
using SchoolEnrollmentAPI.Repositories;

namespace SchoolEnrollmentAPI.Services
{
    /// <summary>
    /// Coordena as operações de alunos e isola o controller da persistência.
    /// </summary>
    public class AlunoService
    {
        private readonly AlunoRepository _repo = new AlunoRepository();

        public PagedResult<Aluno> Listar(string name, int page, int pageSize)
        {
            return _repo.Listar(name, page, pageSize);
        }

        public Aluno Obter(int id) => _repo.Obter(id);

        public Aluno Criar(AlunoRequest request)
        {
            var aluno = Converter(request);
            aluno.Id = _repo.Criar(aluno);
            return aluno;
        }

        public bool Atualizar(int id, AlunoRequest request) => _repo.Atualizar(id, Converter(request));
        public bool Excluir(int id) => _repo.Desativar(id);

        private static Aluno Converter(AlunoRequest request)
        {
            // A camada de serviço evita que o controller conheça o modelo persistido.
            return new Aluno { Nome = request.Nome.Trim(), Email = request.Email.Trim(), DataNascimento = request.DataNascimento.Value, Ativo = true };
        }
    }
}
