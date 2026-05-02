using StudyFlow.Data;
using StudyFlow.Data.Models;
using System.Collections.ObjectModel;

namespace StudyFlow.Views.Tarefas;

public partial class MinhasTarefasPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly Usuario _usuario;

    public MinhasTarefasPage(Usuario usuario)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _usuario = usuario;

        CarregarTarefas();
    }

    private async void CarregarTarefas()
    {
        try
        {
            await _db.InitAsync();

            // 1. Pede ID do Aluno vinculado a este Usuário
            var alunos = await _db.ListarAlunosAsync();
            var alunoLogado = alunos.FirstOrDefault(a => a.IdUsuario == _usuario.IdUsuario);

            if (alunoLogado == null)
            {
                await DisplayAlert("Erro", "Perfil de aluno não encontrado.", "OK");
                return;
            }

            // 2. Busca todas as tarefas
            var todasTarefas = await _db.ListarTarefasAsync();

            // 3. Filtrar as tarefas que pertencem a este aluno (ou à turma dele)
            var tarefasDoAluno = todasTarefas.Where(t => t.IdAluno == alunoLogado.IdAluno).ToList();

            // Atualizar a lista na tela
            listTarefas.ItemsSource = tarefasDoAluno;
            lblQuantidade.Text = $"{tarefasDoAluno.Count} tarefas pendentes";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Não foi possível carregar as tarefas.", "OK");
        }
    }
}