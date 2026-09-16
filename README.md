# School Enrollment API

API REST para controle de alunos, turmas e matrículas escolares. Este projeto foi desenvolvido como teste prático com .NET Framework 4.8, ASP.NET Web API, SQL Server e Dapper.

O objetivo deste README é permitir executar o projeto do zero e servir como guia de estudo das decisões tomadas.

## Tecnologias

- .NET Framework 4.8
- ASP.NET Web API 2 hospedada pelo IIS Express
- SQL Server LocalDB, SQL Server Express ou SQL Server completo
- Dapper para acesso a dados com SQL manual
- Swagger/Swashbuckle para documentação e teste interativo da API
- MSTest para testes unitários

Não há Entity Framework ou outro ORM gerador de SQL no projeto.

## Pré-requisitos

Instale os itens abaixo antes de executar:

1. Visual Studio com o workload de desenvolvimento ASP.NET e Web.
2. .NET Framework 4.8 Targeting Pack.
3. SQL Server LocalDB ou outra instância de SQL Server.
4. SQL Server Management Studio (SSMS), recomendado para executar o script do banco.

O projeto foi configurado inicialmente para a instância LocalDB padrão:

```text
(localdb)\MSSQLLocalDB
```

## Criar o banco de dados

1. Abra o SSMS.
2. Conecte em `(localdb)\MSSQLLocalDB` usando **Autenticação do Windows**.
3. Abra o arquivo [script-banco.sql](script-banco.sql).
4. Execute todo o conteúdo com `F5` ou pelo botão **Executar**.
5. Atualize a pasta **Bancos de Dados** e confirme a criação de `TesteEscola`.

> Atenção: o script remove as tabelas `Matricula`, `Aluno` e `Turma` antes de recriá-las. Use-o apenas em ambiente de desenvolvimento ou quando puder perder os dados atuais.

O script cria as tabelas e inclui alunos, turmas e matrículas de exemplo.

## Configurar a connection string

A connection string está em [Web.config](SchoolEnrollmentAPI/Web.config):

```xml
Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=TesteEscola;Integrated Security=True;
```

Para SQL Server Express, um exemplo seria:

```xml
Data Source=.\SQLEXPRESS;Initial Catalog=TesteEscola;Integrated Security=True;
```

Para outra instância, altere apenas o valor de `Data Source`. Senhas e connection strings de ambientes reais não devem ser versionadas no Git.

## Executar a API

1. Abra [SchoolEnrollmentAPI.sln](SchoolEnrollmentAPI.sln) no Visual Studio.
2. Restaure os pacotes NuGet caso o Visual Studio solicite.
3. Defina `SchoolEnrollment.WebApi` como projeto de inicialização, se necessário.
4. Pressione `F5` ou clique em **IIS Express**.

A aplicação inicia na documentação Swagger:

```text
http://localhost:50731/swagger
```

Se a porta escolhida pelo IIS Express for diferente, use a URL exibida pelo Visual Studio e acrescente `/swagger`.

## Endpoints

### Alunos

| Método | Rota | Descrição |
| --- | --- | --- |
| GET | `/api/alunos` | Lista alunos ativos com paginação e filtro opcional. |
| GET | `/api/alunos/{id}` | Busca um aluno ativo pelo identificador. |
| POST | `/api/alunos` | Cria um aluno. |
| PUT | `/api/alunos/{id}` | Atualiza um aluno ativo. |
| DELETE | `/api/alunos/{id}` | Faz exclusão lógica, alterando `Ativo` para `false`. |

Exemplo de paginação e filtro:

```text
GET /api/alunos?page=1&pageSize=10&name=ana
```

Exemplo de criação/atualização:

```json
{
  "nome": "Mariana Costa",
  "email": "mariana.costa@email.com",
  "dataNascimento": "2006-04-10"
}
```

### Turmas

| Método | Rota | Descrição |
| --- | --- | --- |
| GET | `/api/turmas` | Lista turmas, vagas totais e vagas restantes. |

O campo `remainingSeats` vem diretamente da coluna `VagasDisponiveis` no SQL Server.

### Matrículas

| Método | Rota | Descrição |
| --- | --- | --- |
| POST | `/api/matriculas` | Cria uma matrícula quando as regras de negócio permitem. |

Corpo da requisição:

```json
{
  "studentId": 1,
  "classroomId": 2
}
```

Regras aplicadas:

- O aluno precisa existir e estar ativo.
- A turma precisa existir e ter vagas disponíveis.
- O aluno não pode ter matrícula duplicada na mesma turma.
- A inserção em `Matricula` e a redução de `Turma.VagasDisponiveis` ocorrem na mesma transação SQL.

A transação usa isolamento `Serializable` e bloqueios `UPDLOCK`/`HOLDLOCK`, protegendo a validação contra requisições concorrentes que poderiam consumir a mesma vaga.

### Relatório

| Método | Rota | Descrição |
| --- | --- | --- |
| GET | `/api/relatorios/alunos-por-turma` | Retorna nome da turma, total de alunos matriculados e vagas restantes. |

O relatório é calculado no SQL Server por meio de `LEFT JOIN`, `COUNT` e `GROUP BY`. O `LEFT JOIN` garante que turmas sem alunos também apareçam no resultado.

## Status HTTP

| Status | Situação |
| --- | --- |
| `200 OK` | Consulta, atualização ou exclusão lógica concluída. |
| `201 Created` | Aluno ou matrícula criado com sucesso. |
| `400 Bad Request` | Dados obrigatórios, formato de e-mail ou paginação inválidos. |
| `404 Not Found` | Aluno ou turma não encontrado. |
| `409 Conflict` | Regra de matrícula impediu a operação: aluno inativo, turma lotada ou duplicidade. |

## Organização do código

```text
Controllers/     Camada HTTP: rotas, validação do corpo e status de resposta.
Services/        Casos de uso e regras de negócio.
Repositories/    SQL manual e chamadas Dapper.
Models/          Contratos de entrada, saída e entidades.
Infrastructure/  Criação da conexão com SQL Server.
App_Start/       Configuração de Web API e Swagger.
```

Os controllers não conhecem SQL. Eles delegam o trabalho aos services, que por sua vez usam os repositories. Essa separação facilita manutenção, testes e evolução das regras.

## Testes unitários

O projeto [SchoolEnrollmentAPI.Tests](SchoolEnrollmentAPI.Tests) usa MSTest e valida a regra de matrícula sem SQL Server.

Os cenários cobertos são:

1. Matrícula criada com sucesso.
2. Aluno inativo gera conflito.
3. Turma sem vagas gera conflito.
4. Matrícula duplicada gera conflito.
5. Aluno inexistente gera não encontrado.

Para executar no Visual Studio:

1. Abra **Teste** → **Gerenciador de Testes**.
2. Clique em **Executar Tudo**.
3. O resultado esperado é `5 Aprovado` e `0 Falhou`.

Os testes usam `FakeEnrollmentRepository`, um repositório falso que controla o resultado da persistência. Assim, o foco fica somente nas regras do `EnrollmentService`, sem depender de banco, rede ou dados prévios.

## Roteiro de teste manual no Swagger

1. Execute `GET /api/alunos` e confirme que os alunos ativos são retornados.
2. Execute `GET /api/turmas` e observe `remainingSeats`.
3. Execute `POST /api/matriculas` com um aluno ativo e uma turma com vaga.
4. Execute novamente `GET /api/turmas`: a vaga da turma usada deve diminuir em um.
5. Repita a mesma matrícula: a API deve retornar `409 Conflict`.
6. Tente matricular o aluno inativo de id `4`: a API deve retornar `409 Conflict`.
7. Execute `GET /api/relatorios/alunos-por-turma` e confirme os totais.

## Observação sobre a entrega

O README foi preenchido na etapa final para refletir o estado completo da API. O histórico Git foi dividido por incrementos funcionais: CRUD de alunos, turmas, matrícula transacional, relatório, Swagger e testes.
