using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.SecretariaPages;

public partial class RelatoriosGeraisPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db = new();

    public RelatoriosGeraisPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await CarregarRelatorios();
    }

    private async Task CarregarRelatorios()
    {
        await _db.InitAsync();

        var usuarios = await _db.ListarUsuariosAsync();
        var alunos = await _db.ListarAlunosAsync();
        var professores = await _db.ListarProfessoresAsync();
        var responsaveis = await _db.ListarResponsaveisAsync();
        var turmas = await _db.ListarTurmasAsync();

        lblTotalAlunos.Text = $"Total de alunos: {alunos.Count}";
        lblTotalProfessores.Text = $"Total de professores: {professores.Count}";
        lblTotalResponsaveis.Text = $"Total de responsáveis: {responsaveis.Count}";
        lblTotalTurmas.Text = $"Total de turmas: {turmas.Count}";
        lblMatriculados.Text = $"Alunos matriculados: {alunos.Count(a => !string.IsNullOrEmpty(a.Turma))}";
        lblSemTurma.Text = $"Alunos sem turma: {alunos.Count(a => string.IsNullOrEmpty(a.Turma))}";

        var relatorioTurmas = turmas.Select(t =>
        {
            var alunosDaTurma = alunos
                .Where(a => a.Turma == t.Nome)
                .Select(a =>
                {
                    var user = usuarios.FirstOrDefault(u => u.IdUsuario == a.IdUsuario);
                    return user?.Nome ?? "Sem nome";
                });

            return new
            {
                Turma = t.Nome,
                Alunos = alunosDaTurma.Any()
                    ? string.Join(", ", alunosDaTurma)
                    : "Nenhum aluno matriculado"
            };
        }).ToList();

        listaRelatorioTurmas.ItemsSource = relatorioTurmas;
    }

    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}