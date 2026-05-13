using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.SecretariaPages;

public partial class ListarMatriculas : ContentPage
{
    private readonly StudyFlowDatabaseService _db = new();

    private Aluno? _alunoSelecionado;

    public ListarMatriculas()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await CarregarAlunos();
    }

    private async Task CarregarAlunos()
    {
        await _db.InitAsync();

        var alunos = await _db.ListarAlunosAsync();

        var usuarios = await _db.ListarUsuariosAsync();

        var lista = alunos.Select(a =>
        {
            var user = usuarios.FirstOrDefault(u => u.IdUsuario == a.IdUsuario);

            return new
            {
                a.IdAluno,
                a.CPF,
                a.Turma,
                a.IdUsuario,
                Nome = user?.Nome ?? "Sem nome"
            };
        })
        .OrderBy(x => x.Nome)
        .ToList();

        listaAlunos.ItemsSource = lista;
    }

    private async void OnAlunoSelecionado(object sender, SelectionChangedEventArgs e)
    {

        var item = e.CurrentSelection.FirstOrDefault();

        if (item == null)
            return;

        var type = item.GetType();

        int idAluno = (int)type.GetProperty("IdAluno")!.GetValue(item)!;
        string nome = (string)type.GetProperty("Nome")!.GetValue(item)!;
        string cpf = (string)type.GetProperty("CPF")!.GetValue(item)!;
        string turma = (string)type.GetProperty("Turma")!.GetValue(item)!;

        _alunoSelecionado = new Aluno
        {
            IdAluno = idAluno,
            CPF = cpf,
            Turma = turma
        };

        lblNome.Text = nome; 
        lblCPF.Text = cpf;
        lblTurma.Text = turma;

        listaAlunos.IsVisible = false;

        pickerTurmaEditar.ItemsSource = (await _db.ListarTurmasAsync())
            .Select(t => t.Nome)
            .ToList();
    }

    private async void OnSalvarMatricula(object sender, EventArgs e)
    {
        if (_alunoSelecionado == null)
            return;

        if (pickerTurmaEditar.SelectedIndex == -1)
        {
            await DisplayAlert("Erro", "Selecione uma turma", "OK");
            return;
        }

        _alunoSelecionado.Turma = pickerTurmaEditar.SelectedItem.ToString();

        await _db.UpdateAsync(_alunoSelecionado);

        await DisplayAlert("Sucesso", "Matrícula atualizada", "OK");

        await CarregarAlunos();
    }

    private async void OnExcluirAluno(object sender, EventArgs e)
    {
        if (_alunoSelecionado == null)
            return;

        bool confirm = await DisplayAlert("Excluir", "Deseja excluir este aluno?", "Sim", "Não");

        if (!confirm)
            return;

        await _db.DeletarAlunoAsync(_alunoSelecionado);

        await DisplayAlert("Sucesso", "Aluno excluído", "OK");

        _alunoSelecionado = null;

        await CarregarAlunos();
    }
}