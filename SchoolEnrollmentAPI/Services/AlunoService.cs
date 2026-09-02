using SchoolEnrollmentAPI.Models;
using SchoolEnrollmentAPI.Repositories;

namespace SchoolEnrollmentAPI.Services
{
    public class AlunoService
    {
        private readonly AlunoRepository _repo = new AlunoRepository();

        public PagedResult<Aluno> Listar(string nome, int pagina, int tamanho)
        {
            return _repo.Listar(nome, pagina, tamanho);
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
            return new Aluno { Nome = request.Nome.Trim(), Email = request.Email.Trim(), DataNascimento = request.DataNascimento.Value, Ativo = true };
        }
    }
}
