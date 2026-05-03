using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views;

public partial class TarefasAlunoPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly Usuario _usuario;

    public TarefasAlunoPage(Usuario usuario)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _usuario = usuario;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var tarefas = await _db.ListarTarefasAsync();

        // BUSCA O ALUNO LOGADO
        var alunos = await _db.ListarAlunosAsync();

        var aluno = alunos.FirstOrDefault(a => a.IdUsuario == _usuario.IdUsuario);

        if (aluno == null)
        {
            listaTarefas.ItemsSource = new List<Tarefa>();
            return;
        }

        // FILTRA PELA TURMA
        listaTarefas.ItemsSource = tarefas
            .Where(t => t.Turma == aluno.Turma)
            .ToList();
    }

    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
