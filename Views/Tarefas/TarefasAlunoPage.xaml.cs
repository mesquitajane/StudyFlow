using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.Tarefas;

public partial class TarefasAlunoPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly Usuario _usuarioLogado;

    public TarefasAlunoPage(Usuario usuario)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _usuarioLogado = usuario;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarDados();
    }

    private async Task CarregarDados()
    {
        await _db.InitAsync();

        var alunos = await _db.ListarAlunosAsync();
        var alunoAtivo = alunos.FirstOrDefault(a => a.IdUsuario == _usuarioLogado.IdUsuario);

        if (alunoAtivo != null)
        {
            var todasTarefas = await _db.ListarTarefasAsync();
            var todasEntregas = await _db.ListarEntregasAsync();

            // Unindo as duas tabelas na classe de visualização
            var listaExibicao = todasTarefas
                .Where(t => t.Turma == alunoAtivo.Turma)
                .Select(t => new TarefaComNota
                {
                    Tarefa = t,
                    Entrega = todasEntregas.FirstOrDefault(e => e.IdTarefa == t.IdTarefa && e.IdAluno == alunoAtivo.IdAluno)
                }).ToList();

            listaTarefas.ItemsSource = listaExibicao;
        }
    }

    private async void OnTarefaTapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var selecionado = (TarefaComNota)frame.BindingContext;

        if (selecionado != null)
        {
            var alunos = await _db.ListarAlunosAsync();
            var alunoAtivo = alunos.FirstOrDefault(a => a.IdUsuario == _usuarioLogado.IdUsuario);

            // Passamos a Tarefa original e o Aluno para a página de entrega
            await Navigation.PushAsync(new EntregarTarefaPage(selecionado.Tarefa, alunoAtivo));
        }
    }

    private async void OnVoltarClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}