using StudyFlow.Data.Models;
using StudyFlow.Data;

namespace StudyFlow.Views.Relatorios;

public partial class LancamentoNotasPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly Usuario _professorLogado;
    private List<Aluno> _todosAlunos;

    public LancamentoNotasPage(Usuario professor)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _professorLogado = professor;
        CarregarTurmas();
    }

    private async void CarregarTurmas()
    {
        await _db.InitAsync();
        _todosAlunos = await _db.ListarAlunosAsync();
        var turmas = _todosAlunos.Select(a => a.Turma).Distinct().ToList();
        pickerTurma.ItemsSource = turmas;
    }

    private void OnTurmaSelected(object sender, EventArgs e)
    {
        var turmaSelecionada = pickerTurma.SelectedItem as string;
        if (!string.IsNullOrEmpty(turmaSelecionada))
        {
            pickerAluno.ItemsSource = _todosAlunos.Where(a => a.Turma == turmaSelecionada).ToList();
            pickerAluno.ItemDisplayBinding = new Binding("IdAluno");
            pickerAluno.IsEnabled = true;
        }
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        var aluno = pickerAluno.SelectedItem as Aluno;
        if (aluno == null || string.IsNullOrWhiteSpace(entryNota.Text))
        {
            await DisplayAlert("Erro", "Preencha todos os campos.", "OK");
            return;
        }

        double notaValida = double.Parse(entryNota.Text);

        var novoDesempenho = new Desempenho
        {
            IdAluno = aluno.IdAluno,
            IdProfessor = _professorLogado.IdUsuario,
            Nota = notaValida,
            TipoAvaliacao = entryTipo.Text
        };

        await _db.SalvarDesempenhoAsync(novoDesempenho);
        await DisplayAlert("Sucesso", "Nota lançada com sucesso!", "OK");
        await Navigation.PopAsync();
    }

    private async void OnVoltarClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}