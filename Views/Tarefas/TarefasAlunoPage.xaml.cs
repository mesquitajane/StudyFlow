using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.Tarefas;

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
        await _db.InitAsync();

        var alunos = await _db.ListarAlunosAsync();
        var alunoLogado = alunos.FirstOrDefault(a => a.IdUsuario == _usuario.IdUsuario);

        if (alunoLogado != null)
        {
            lblTurma.Text = $"Turma: {alunoLogado.Turma}";
            var todas = await _db.ListarTarefasAsync();
            // FILTRO: Onde a Turma da tarefa é igual a Turma do aluno
            var tarefasDaTurma = todas.Where(t => t.Turma == alunoLogado.Turma).ToList();

            listaTarefas.ItemsSource = tarefasDaTurma;
        }
    }

    private async void OnVoltarClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}