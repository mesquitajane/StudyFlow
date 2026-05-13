using System.Linq;
using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.SecretariaPages;

public partial class MatricularAlunoPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db = new();

    private List<Aluno> _listaFiltrada = new();
    private List<Turma> _turmas = new();
    private dynamic? _alunoSelecionado;

    public MatricularAlunoPage()
    {
        InitializeComponent();
        CarregarTurmas();
    }

    
    private async void CarregarTurmas()
    {
        await _db.InitAsync();

        _turmas = await _db.ListarTurmasAsync();

        pickerTurma.ItemsSource = _turmas.Select(t => t.Nome).ToList();
    }

    
   
    private async void OnBuscarAluno(object sender, TextChangedEventArgs e)
    {
        string texto = e.NewTextValue?.Trim().ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(texto))
        {
            listaAlunos.IsVisible = false;
            listaAlunos.ItemsSource = null;
            return;
        }

        var alunos = await _db.ListarAlunosAsync();
        var usuarios = await _db.ListarUsuariosAsync();

        var filtrados = alunos
            .Where(a =>
                a.CPF.Contains(texto) ||
                usuarios.Any(u => u.IdUsuario == a.IdUsuario &&
                                 u.Nome.ToLower().Contains(texto)))
            .Select(a =>
            {
                var user = usuarios.FirstOrDefault(u => u.IdUsuario == a.IdUsuario);

                return new
                {
                    a.IdAluno,
                    Nome = user?.Nome ?? "Sem nome",
                    a.CPF,
                    a.Turma,
                    Email = user?.Email ?? ""
                };
            })
            .ToList();

        listaAlunos.ItemsSource = filtrados;
        listaAlunos.IsVisible = filtrados.Any();
    }
    private async void OnAlunoSelecionado(object sender, SelectionChangedEventArgs e)
    {
        _alunoSelecionado = e.CurrentSelection.FirstOrDefault();

        if (_alunoSelecionado == null)
            return;

        lblSelecionado.Text = $"{_alunoSelecionado.Nome} - {_alunoSelecionado.CPF}";

        listaAlunos.IsVisible = false;
    }

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

        var alunos = await _db.ListarAlunosAsync();

        var aluno = alunos.FirstOrDefault(a =>
            a.IdAluno == (int)_alunoSelecionado.IdAluno);

        if (aluno == null)
        {
            await DisplayAlert("Erro", "Aluno não encontrado", "OK");
            return;
        }

        if (!string.IsNullOrEmpty(aluno.Turma))
        {
            await DisplayAlert("Erro", "Aluno já está matriculado em uma turma", "OK");
            return;
        }

        aluno.Turma = pickerTurma.SelectedItem.ToString();

        await _db.UpdateAsync(aluno);

        await DisplayAlert("Sucesso", "Aluno matriculado com sucesso!", "OK");

        // limpa tela
        _alunoSelecionado = null;
        lblSelecionado.Text = "Nenhum aluno selecionado";
        entryCpf.Text = "";
        listaAlunos.IsVisible = false;
    }
}