using System.Linq;
using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.SecretariaPages;

public partial class MatricularAlunoPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db = new();

    private List<Aluno> _listaFiltrada = new();
    private Aluno? _alunoSelecionado;
    private List<Turma> _turmas = new();

    public MatricularAlunoPage()
    {
        InitializeComponent();
        CarregarTurmas();
    }

    // -------------------------
    // CARREGAR TURMAS
    // -------------------------
    private async void CarregarTurmas()
    {
        await _db.InitAsync();

        _turmas = await _db.ListarTurmasAsync();

        pickerTurma.ItemsSource = _turmas.Select(t => t.Nome).ToList();
    }

    // -------------------------
    // BUSCAR ALUNO (CPF OU NOME)
    // -------------------------
    private async void OnBuscarAluno(object sender, TextChangedEventArgs e)
    {
        string texto = e.NewTextValue?.Trim().ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(texto))
        {
            listaAlunos.IsVisible = false;
            return;
        }

        var alunos = await _db.ListarAlunosAsync();
        var usuarios = await _db.ListarUsuariosAsync();

        _listaFiltrada = alunos.Where(a =>
        {
            var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == a.IdUsuario);

            return a.CPF.Contains(texto) ||
                   (usuario?.Nome?.ToLower().Contains(texto) ?? false);
        }).ToList();

        listaAlunos.ItemsSource = _listaFiltrada;
        listaAlunos.IsVisible = _listaFiltrada.Any();
    }

    // -------------------------
    // SELECIONAR ALUNO
    // -------------------------
    private async void OnAlunoSelecionado(object sender, SelectionChangedEventArgs e)
    {
        _alunoSelecionado = e.CurrentSelection.FirstOrDefault() as Aluno;

        if (_alunoSelecionado == null)
            return;

        var usuarios = await _db.ListarUsuariosAsync();
        var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == _alunoSelecionado.IdUsuario);

        entryCpf.Text = _alunoSelecionado.CPF;

        lblSelecionado.Text = usuario?.Nome ?? "Nome não encontrado";

        listaAlunos.IsVisible = false;
        listaAlunos.SelectedItem = null;
    }

    // -------------------------
    // MATRICULAR ALUNO
    // -------------------------
    private async void OnMatricularClicked(object sender, EventArgs e)
    {
        if (_alunoSelecionado == null)
        {
            await DisplayAlert("Erro", "Selecione um aluno", "OK");
            return;
        }

        if (pickerTurma.SelectedIndex == -1)
        {
            await DisplayAlert("Erro", "Selecione uma turma", "OK");
            return;
        }

        // REGRA: não permitir duplicar matrícula
        if (!string.IsNullOrEmpty(_alunoSelecionado.Turma))
        {
            await DisplayAlert("Erro", "Este aluno já está matriculado em uma turma", "OK");
            return;
        }

        _alunoSelecionado.Turma = pickerTurma.SelectedItem.ToString();

        await _db.UpdateAsync(_alunoSelecionado);

        await DisplayAlert("Sucesso", "Aluno matriculado com sucesso!", "OK");

        // limpar tela
        _alunoSelecionado = null;
        lblSelecionado.Text = "Nenhum aluno selecionado";
        entryCpf.Text = string.Empty;
        pickerTurma.SelectedIndex = -1;
        listaAlunos.IsVisible = false;
    }
}