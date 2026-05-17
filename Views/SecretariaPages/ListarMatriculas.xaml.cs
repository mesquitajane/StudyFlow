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

        var lista = alunos
            .Join(
                usuarios,
                aluno => aluno.IdUsuario,
                usuario => usuario.IdUsuario,
                (aluno, usuario) => new
                {
                    aluno.IdAluno,
                    aluno.CPF,
                    aluno.Turma,
                    aluno.IdUsuario,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                })
            .OrderBy(x => x.Nome)
            .ToList();

        listaAlunos.ItemsSource = null;
        listaAlunos.ItemsSource = lista;
    }

    private async void OnAlunoSelecionado(object sender, SelectionChangedEventArgs e)
    {

        var item = e.CurrentSelection.FirstOrDefault();

        if (item == null)
            return;

        var type = item.GetType();

        int idAluno = (int)type.GetProperty("IdAluno")!.GetValue(item)!;

        var alunos = await _db.ListarAlunosAsync();

        _alunoSelecionado = alunos.FirstOrDefault(a => a.IdAluno == idAluno);

        if (_alunoSelecionado == null)
            return;

        var usuarios = await _db.ListarUsuariosAsync();

        var user = usuarios.FirstOrDefault(u => u.IdUsuario == _alunoSelecionado.IdUsuario);

        lblNome.Text = user?.Nome ?? "Sem nome";
        lblCPF.Text = _alunoSelecionado.CPF;
        lblTurma.Text = _alunoSelecionado.Turma;
        lblEmail.Text = user?.Email ?? "Sem email";

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

    private async void OnPesquisarAlunoTextChanged(object sender, TextChangedEventArgs e)
    {
        string texto = e.NewTextValue?.Trim().ToLower() ?? "";

        var alunos = await _db.ListarAlunosAsync();
        var usuarios = await _db.ListarUsuariosAsync();

        var lista = alunos
            .Where(a =>
            {
                var user = usuarios.FirstOrDefault(u => u.IdUsuario == a.IdUsuario);

                return a.CPF.Contains(texto) ||
                       (user != null && user.Nome.ToLower().Contains(texto));
            })
            .Select(a =>
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

        listaAlunos.ItemsSource = null;
        listaAlunos.ItemsSource = lista;
    }
}