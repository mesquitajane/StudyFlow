using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.Relatorios;

public partial class CriarRelatorioPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private List<Aluno> _todosAlunos;
    private Professor _professorLogado;

    public CriarRelatorioPage(Usuario professorUsuario)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        CarregarDadosIniciais(professorUsuario);
    }

    private async void CarregarDadosIniciais(Usuario user)
    {
        await _db.InitAsync();

        // 1. Pega os dados do professor
        var professores = await _db.ListarProfessoresAsync();
        _professorLogado = professores.FirstOrDefault(p => p.IdUsuario == user.IdUsuario);

        // 2. Carrega todos os alunos e extrai as turmas únicas
        _todosAlunos = await _db.ListarAlunosAsync();
        var turmas = _todosAlunos.Select(a => a.Turma).Distinct().ToList();

        pickerTurma.ItemsSource = turmas;
    }

    // Chamado quando o professor escolhe uma turma
    private void OnTurmaSelected(object sender, EventArgs e)
    {
        var turmaSelecionada = pickerTurma.SelectedItem?.ToString();

        if (!string.IsNullOrEmpty(turmaSelecionada))
        {
            // Filtra apenas alunos daquela turma
            var alunosFiltrados = _todosAlunos
                .Where(a => a.Turma == turmaSelecionada)
                .ToList();

            pickerAluno.ItemsSource = alunosFiltrados;
            pickerAluno.ItemDisplayBinding = new Binding("IdAluno"); // Ideal trocar para o Nome depois
            pickerAluno.IsEnabled = true;
        }
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        var alunoSelecionado = (Aluno)pickerAluno.SelectedItem;

        if (alunoSelecionado == null || string.IsNullOrWhiteSpace(editorRelato.Text))
        {
            await DisplayAlert("Erro", "Selecione um aluno e escreva o relato.", "OK");
            return;
        }

        var novoRelatorio = new RelatorioComportamental
        {
            IdAluno = alunoSelecionado.IdAluno,
            IdProfessor = _professorLogado.IdProfessor,
            Comportamento = editorRelato.Text,
            DataRegistro = DateTime.Now
        };

        await _db.SalvarRelatorioAsync(novoRelatorio);
        await DisplayAlert("Sucesso", "Relatório salvo!", "OK");
        await Navigation.PopAsync();
    }

    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}