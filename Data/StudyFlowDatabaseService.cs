using SQLite;
using StudyFlow.Data.Models;

namespace StudyFlow.Data;

public class StudyFlowDatabaseService
{
    private SQLiteAsyncConnection? _database;

    public async Task InitAsync()
    {
        if (_database != null)
            return;

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "studyflow4.db3");
        _database = new SQLiteAsyncConnection(dbPath);

        await _database.CreateTableAsync<Usuario>();
        await _database.CreateTableAsync<Aluno>();
        await _database.CreateTableAsync<Professor>();
        await _database.CreateTableAsync<Responsavel>();
        await _database.CreateTableAsync<Secretaria>();
        await _database.CreateTableAsync<Tarefa>();
        await _database.CreateTableAsync<Desempenho>();
        await _database.CreateTableAsync<RelatorioComportamental>();
        await _database.CreateTableAsync<Entrega>();
    }

    public async Task<int> InserirUsuarioAsync(Usuario usuario)
    {
        await InitAsync();
        return await _database!.InsertAsync(usuario);
    }

    public async Task<int> InserirAlunoAsync(Aluno aluno)
    {
        await InitAsync();
        return await _database!.InsertAsync(aluno);
    }

    public async Task<int> InserirProfessorAsync(Professor professor)
    {
        await InitAsync();
        return await _database!.InsertAsync(professor);
    }

    public async Task<int> InserirResponsavelAsync(Responsavel responsavel)
    {
        await InitAsync();
        return await _database!.InsertAsync(responsavel);
    }

    public async Task<int> InserirSecretariaAsync(Secretaria secretaria)
    {
        await InitAsync();
        return await _database!.InsertAsync(secretaria);
    }

    public async Task<int> InserirTarefaAsync(Tarefa tarefa)
    {
        await InitAsync();
        return await _database!.InsertAsync(tarefa);
    }

    public async Task<List<Usuario>> ListarUsuariosAsync()
    {
        await InitAsync();
        return await _database!.Table<Usuario>().ToListAsync();
    }

    public async Task<List<Professor>> ListarProfessoresAsync()
    {
        await InitAsync();
        return await _database!.Table<Professor>().ToListAsync();
    }
    public async Task<List<Tarefa>> ListarTarefasAsync()
    {
        await InitAsync();
        return await _database!.Table<Tarefa>().ToListAsync();
    }
    public async Task DeletarTarefaAsync(int id)
    {
        await InitAsync();
        var tarefa = await _database.Table<Tarefa>()
                                    .FirstOrDefaultAsync(t => t.IdTarefa == id);
        if (tarefa != null)
            await _database.DeleteAsync(tarefa);
    }
    public async Task UpdateAsync(Tarefa tarefa)
    {
        await InitAsync();
        await _database.UpdateAsync(tarefa);
    }
    public async Task<List<Aluno>> ListarAlunosAsync()
    {
        await InitAsync();
        return await _database!.Table<Aluno>().ToListAsync();
    }

    public async Task<List<Entrega>> ListarEntregasAsync()
    {
        await InitAsync();
        return await _database!.Table<Entrega>().ToListAsync();
    }

    public async Task SalvarEntregaAsync(Entrega entrega)
    {
        await InitAsync();
        // Verifica se já existe uma entrega desse aluno para essa tarefa
        var existente = await _database!.Table<Entrega>()
            .FirstOrDefaultAsync(e => e.IdTarefa == entrega.IdTarefa && e.IdAluno == entrega.IdAluno);

        if (existente != null)
        {
            entrega.IdEntrega = existente.IdEntrega;
            await _database.UpdateAsync(entrega);
        }
        else
        {
            await _database.InsertAsync(entrega);
        }
    }

    public async Task AtualizarEntregaAsync(Entrega entrega)
    {
        await InitAsync();
        await _database!.UpdateAsync(entrega);
    }

    public async Task DeletarUsuarioAsync(int id)
    {
        await InitAsync();

        var usuario = await _database!.Table<Usuario>()
                                      .FirstOrDefaultAsync(u => u.IdUsuario == id);

        if (usuario != null)
            await _database.DeleteAsync(usuario);
    }

    public async Task AtualizarUsuarioAsync(Usuario usuario)
    {
        await InitAsync();
        await _database!.UpdateAsync(usuario);
    }

    public async Task<int> SalvarRelatorioAsync(RelatorioComportamental relatorio)
    {
        await InitAsync();
        return await _database!.InsertAsync(relatorio);
    }

    public async Task<List<RelatorioComportamental>> ListarRelatoriosPorAlunoAsync(int idAluno)
    {
        await InitAsync();
        return await _database!.Table<RelatorioComportamental>()
                               .Where(r => r.IdAluno == idAluno)
                               .OrderByDescending(r => r.DataRegistro)
                               .ToListAsync();
    }

    public async Task<List<RelatorioComportamentalView>> ListarRelatoriosDetalhesAsync(int idAluno)
    {
        await InitAsync();

        // Busca os relatórios do aluno
        var relatorios = await _database.Table<RelatorioComportamental>()
                                        .Where(r => r.IdAluno == idAluno)
                                        .ToListAsync();

        var professores = await ListarProfessoresAsync();
        var usuarios = await ListarUsuariosAsync();

        // Faz um "Join" em memória para pegar o nome do professor
        return relatorios.Select(r => {
            var prof = professores.FirstOrDefault(p => p.IdProfessor == r.IdProfessor);
            var userProf = usuarios.FirstOrDefault(u => u.IdUsuario == prof?.IdUsuario);

            return new RelatorioComportamentalView
            {
                Relatorio = r,
                NomeProfessor = userProf?.Nome ?? "Professor Removido"
            };
        }).OrderByDescending(x => x.Relatorio.DataRegistro).ToList();
    }

    public async Task<Aluno> BuscarAlunoPorCpfAsync(string cpf)
    {
        await InitAsync();
        return await _database.Table<Aluno>()
                              .FirstOrDefaultAsync(a => a.CPF == cpf);
    }

    public async Task<List<Responsavel>> ListarResponsaveisAsync()
    {
        await InitAsync();
        return await _database.Table<Responsavel>().ToListAsync();
    }

    public async Task<int> UpdateAsync<T>(T entity) where T : new()
    {
        await InitAsync();
        return await _database.UpdateAsync(entity);
    }

    public async Task<int> SalvarDesempenhoAsync(Desempenho desempenho)
    {
        await InitAsync();
        return await _database.InsertAsync(desempenho);
    }

    public async Task<List<Desempenho>> ListarNotasAlunoAsync(int idAluno)
    {
        await InitAsync();
        return await _database.Table<Desempenho>()
                              .Where(d => d.IdAluno == idAluno)
                              .ToListAsync();
    }
}
